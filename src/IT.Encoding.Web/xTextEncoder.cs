using IT.Encoding;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.Encodings.Web;

public static class xTextEncoder
{
    private const int EncodeStartingOutputBufferSize = 1024; // bytes or chars, depending

    public static String Encode(this TextEncoder textEncoder, ReadOnlySpan<char> value)
    {
        int indexOfFirstCharToEncode;
        unsafe
        {
            fixed (char* pText = &MemoryMarshal.GetReference(value))
            {
                indexOfFirstCharToEncode = textEncoder.FindFirstCharacterToEncode(pText, value.Length);
            }
        }

        if (indexOfFirstCharToEncode < 0) return value.ToString();

        // We optimize for the data having no "requires encoding" chars, so keep the
        // real encoding logic out of the fast path.

        return textEncoder.EncodeToNewString(value, indexOfFirstCharToEncode);
    }

    private static String EncodeToNewString(this TextEncoder textEncoder, ReadOnlySpan<char> value, int indexOfFirstCharToEncode)
    {
        ReadOnlySpan<char> remainingInput = value.Slice(indexOfFirstCharToEncode);
        var stringBuilder = new ValueStringBuilder(stackalloc char[EncodeStartingOutputBufferSize]);

#if !NETCOREAPP
        // Can't call string.Concat later in the method, so memcpy now.
        stringBuilder.Append(value.Slice(0, indexOfFirstCharToEncode));
#endif

        // On each iteration of the main loop, we'll make sure we have at least this many chars left in the
        // destination buffer. This should prevent us from making very chatty calls where we only make progress
        // one char at a time.
        int minBufferBumpEachIteration = EncodeStartingOutputBufferSize;

        do
        {
            // AppendSpan mutates the VSB length to include the newly-added span. This potentially overallocates.
            Span<char> destBuffer = stringBuilder.AppendSpan(Math.Max(remainingInput.Length, minBufferBumpEachIteration));
            textEncoder.EncodeCore(remainingInput, destBuffer, out int charsConsumedJustNow, out int charsWrittenJustNow, isFinalBlock: true);
            if (charsWrittenJustNow == 0 || (uint)charsWrittenJustNow > (uint)destBuffer.Length)
            {
                throw new ArgumentException("MaxOutputCharsPerInputChar");
            }
            remainingInput = remainingInput.Slice(charsConsumedJustNow);
            // It's likely we didn't populate the entire span. If this is the case, adjust the VSB length
            // to reflect that there's unused buffer at the end of the VSB instance.
            stringBuilder.Length -= destBuffer.Length - charsWrittenJustNow;
        } while (!remainingInput.IsEmpty);

#if NETCOREAPP
        string retVal = string.Concat(value.Slice(0, indexOfFirstCharToEncode), stringBuilder.AsSpan());
        stringBuilder.Dispose();
        return retVal;
#else
        return stringBuilder.ToString();
#endif
    }

    // skips the call to FindFirstCharacterToEncode
    public static OperationStatus EncodeCore(this TextEncoder textEncoder, ReadOnlySpan<char> source, Span<char> destination, out int charsConsumed, out int charsWritten, bool isFinalBlock)
    {
        int originalSourceLength = source.Length;
        int originalDestinationLength = destination.Length;

        while (!source.IsEmpty)
        {
            OperationStatus status = Rune.DecodeFromUtf16(source, out Rune scalarValue, out int charsConsumedJustNow);
            if (status != OperationStatus.Done)
            {
                if (!isFinalBlock && status == OperationStatus.NeedMoreData)
                {
                    goto NeedMoreData;
                }

                Debug.Assert(scalarValue == Rune.ReplacementChar); // should be replacement char
                goto MustEncode;
            }

            if (!textEncoder.WillEncode(scalarValue.Value))
            {
                if (!scalarValue.TryEncodeToUtf16(destination, out _))
                {
                    goto DestinationTooSmall;
                }
                source = source.Slice(charsConsumedJustNow);
                destination = destination.Slice(charsConsumedJustNow); // reflecting input directly to the output, same # of chars written
                continue;
            }

        MustEncode:

            if (!textEncoder.TryEncodeUnicodeScalar((uint)scalarValue.Value, destination, out int charsWrittenJustNow))
            {
                goto DestinationTooSmall;
            }

            source = source.Slice(charsConsumedJustNow);
            destination = destination.Slice(charsWrittenJustNow);
        }

        // And we're finished!

        OperationStatus retVal = OperationStatus.Done;

    ReturnCommon:
        charsConsumed = originalSourceLength - source.Length;
        charsWritten = originalDestinationLength - destination.Length;
        return retVal;

    NeedMoreData:
        retVal = OperationStatus.NeedMoreData;
        goto ReturnCommon;

    DestinationTooSmall:
        retVal = OperationStatus.DestinationTooSmall;
        goto ReturnCommon;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Boolean TryEncodeUnicodeScalar(this TextEncoder textEncoder, uint unicodeScalar, Span<char> buffer, out int charsWritten)
    {
        fixed (char* pBuffer = &MemoryMarshal.GetReference(buffer))
        {
            return textEncoder.TryEncodeUnicodeScalar((int)unicodeScalar, pBuffer, buffer.Length, out charsWritten);
        }
    }
}
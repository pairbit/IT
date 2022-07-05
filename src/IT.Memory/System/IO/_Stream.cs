#if NETSTANDARD2_0

using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

//https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/IO/Stream.cs
public static class _Stream
{
    public static ValueTask<int> ReadAsync(this Stream stream, Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
        {
            return new ValueTask<int>(stream.ReadAsync(array.Array!, array.Offset, array.Count, cancellationToken));
        }

        byte[] sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
        return FinishReadAsync(stream.ReadAsync(sharedBuffer, 0, buffer.Length, cancellationToken), sharedBuffer, buffer);

        static async ValueTask<int> FinishReadAsync(Task<int> readTask, byte[] localBuffer, Memory<byte> localDestination)
        {
            try
            {
                int result = await readTask.ConfigureAwait(false);
                new ReadOnlySpan<byte>(localBuffer, 0, result).CopyTo(localDestination.Span);
                return result;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(localBuffer);
            }
        }
    }

    public static int Read(this Stream stream, Span<Byte> buffer)
    {
        Byte[] sharedBuffer = ArrayPool<Byte>.Shared.Rent(buffer.Length);
        try
        {
            int numRead = stream.Read(sharedBuffer, 0, buffer.Length);

            if ((uint)numRead > (uint)buffer.Length) throw new IOException("IO_StreamTooLong");

            new ReadOnlySpan<Byte>(sharedBuffer, 0, numRead).CopyTo(buffer);

            return numRead;
        }
        finally
        {
            ArrayPool<Byte>.Shared.Return(sharedBuffer);
        }
    }

    public static ValueTask WriteAsync(this Stream stream, ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (MemoryMarshal.TryGetArray(buffer, out ArraySegment<byte> array))
        {
            return new ValueTask(stream.WriteAsync(array.Array!, array.Offset, array.Count, cancellationToken));
        }

        byte[] sharedBuffer = ArrayPool<byte>.Shared.Rent(buffer.Length);
        buffer.Span.CopyTo(sharedBuffer);
        return new ValueTask(FinishWriteAsync(stream.WriteAsync(sharedBuffer, 0, buffer.Length, cancellationToken), sharedBuffer));
    }

    private static async Task FinishWriteAsync(Task writeTask, byte[] localBuffer)
    {
        try
        {
            await writeTask.ConfigureAwait(false);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(localBuffer);
        }
    }
}

#endif
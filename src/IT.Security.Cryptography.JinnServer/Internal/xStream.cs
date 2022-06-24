using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public static class xStream
{
    public static Int32 GetParts(this Stream stream, Int64 count) => (Int32)Math.Ceiling(stream.Length / (Double)count);

    /// <summary>
    /// Прочитать часть файла
    /// </summary>
    /// <param name="stream">Поток</param>
    /// <param name="count">Количество байтов в части</param>
    /// <param name="part">Часть файла, начиная с 0</param>
    /// <param name="origin">С какой стороны начинать сдвиг</param>
    public static Byte[] ReadPartBytes(this Stream stream, Int64 count, Int32 part, SeekOrigin origin = SeekOrigin.Begin)
    {
        if (part < 0 || part > stream.GetParts(count) - 1) throw new ArgumentOutOfRangeException(nameof(part));

        Int64 offset = count * part;
        stream.Seek(offset, origin);
        Int64 maxLength = stream.Length - offset;
        if (count > maxLength) count = (Int32)maxLength;
        return stream.ReadBytes(0, count);
    }

    /// <inheritdoc cref="ReadPartBytes"/>
    public static Task<Byte[]> ReadPartBytesAsync(this Stream stream, Int64 count, Int32 part, SeekOrigin origin = SeekOrigin.Begin, CancellationToken cancellationToken = default)
    {
        if (part < 0 || part > stream.GetParts(count) - 1) throw new ArgumentOutOfRangeException(nameof(part));

        Int64 offset = count * part;
        stream.Seek(offset, origin);
        Int64 maxLength = stream.Length - offset;
        if (count > maxLength) count = (Int32)maxLength;
        return stream.ReadBytesAsync(0, count, cancellationToken);
    }

    public static Byte[] ReadBytes(this Stream stream, Int32 offset = 0, Int64? count = null)
    {
        var bytes = new Byte[count ?? stream.Length];
        stream.Read(bytes, offset, bytes.Length);
        return bytes;
    }

    public static async Task<Byte[]> ReadBytesAsync(this Stream stream, Int32 offset = 0, Int64? count = null, CancellationToken cancellationToken = default)
    {
        var bytes = new Byte[count ?? stream.Length];
        await stream.ReadAsync(bytes, offset, bytes.Length, cancellationToken).ConfigureAwait(false);
        return bytes;
    }
}
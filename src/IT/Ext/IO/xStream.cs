using IT.Resources;
using IT.Validation;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IT.Ext
{
    public static class xStream
    {
        public static Int32 GetParts(this Stream stream, Int32 count) => xFile.GetParts(stream.Length, count);

        /// <summary>
        /// Прочитать часть файла
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="count">Количество байтов в части</param>
        /// <param name="part">Часть файла, начиная с 0</param>
        /// <param name="origin">С какой стороны начинать сдвиг</param>
        public static Byte[] ReadPartBytes(this Stream stream, Int32 count, Int32 part, SeekOrigin origin = SeekOrigin.Begin)
        {
            Arg.Range(part, 0, stream.GetParts(count) - 1, nameof(part), Res.Get("IO_Part_NotFound"));
            Int64 offset = (Int64)count * part;
            stream.Seek(offset, origin);
            Int64 maxLength = stream.Length - offset;
            if (count > maxLength) count = (Int32)maxLength;
            return stream.ReadBytes(0, count);
        }

        /// <inheritdoc cref="ReadPartBytes"/>
        public static Task<Byte[]> ReadPartBytesAsync(this Stream stream, Int32 count, Int32 part, SeekOrigin origin = SeekOrigin.Begin, CancellationToken cancellationToken = default)
        {
            Arg.Range(part, 0, stream.GetParts(count) - 1, nameof(part), Res.Get("IO_Part_NotFound"));
            Int64 offset = (Int64)count * part;
            stream.Seek(offset, origin);
            Int64 maxLength = stream.Length - offset;
            if (count > maxLength) count = (Int32)maxLength;
            return stream.ReadBytesAsync(0, count, cancellationToken);
        }

        public static Byte[] ReadBytes(this Stream stream, Int32 offset = 0, Int32? count = null)
        {
            var bytes = new Byte[count ?? stream.Length];
            stream.Read(bytes, offset, bytes.Length);
            return bytes;
        }

        public static async Task<Byte[]> ReadBytesAsync(this Stream stream, Int32 offset = 0, Int32? count = null, CancellationToken cancellationToken = default)
        {
            var bytes = new Byte[count ?? stream.Length];
            await stream.ReadAsync(bytes, offset, bytes.Length, cancellationToken).CA();
            return bytes;
        }
    }
}
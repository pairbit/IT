using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    //https://github.com/dotnet/runtime/blob/419e949d258ecee4c40a460fb09c66d974229623/src/libraries/System.IO.FileSystem/src/System/IO/File.cs
    public static class _File
    {
#if NETSTANDARD2_0

        private const Int32 MaxByteArrayLength = 0x7FFFFFC7;

        private const Int32 DefaultBufferSize = 4096;

        private static Encoding _UTF8NoBOM;

        private static Encoding UTF8NoBOM => _UTF8NoBOM ??= new UTF8Encoding(false, true);

#endif

#if NETSTANDARD2_0 || NETSTANDARD2_1

        public static void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            if (overwrite) File.Delete(destFileName);

            File.Move(sourceFileName, destFileName);
        }

#endif

#if NETSTANDARD2_0

        public static Task AppendAllLinesAsync(String path, IEnumerable<String> contents, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = UTF8NoBOM;

            PathNotEmpty(path);
            if (contents == null) throw new ArgumentNullException(nameof(contents));

            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : WriteAllLinesAsync(AsyncStreamWriter(path, encoding, append: true), contents, cancellationToken);
        }

        public static Task WriteAllLinesAsync(String path, IEnumerable<String> contents, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = UTF8NoBOM;

            PathNotEmpty(path);
            if (contents == null) throw new ArgumentNullException(nameof(contents));

            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : WriteAllLinesAsync(AsyncStreamWriter(path, encoding, append: false), contents, cancellationToken);
        }

        public static async Task<String[]> ReadAllLinesAsync(String path, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            PathNotEmpty(path);

            if (cancellationToken.IsCancellationRequested)
                return await Task.FromCanceled<string[]>(cancellationToken).ConfigureAwait(false);

            using var reader = AsyncStreamReader(path, encoding);

            cancellationToken.ThrowIfCancellationRequested();
            string line;
            var lines = new List<string>();
            while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
            {
                lines.Add(line);
                cancellationToken.ThrowIfCancellationRequested();
            }

            return lines.ToArray();
        }

        public static Task AppendAllTextAsync(String path, String contents, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = UTF8NoBOM;

            PathNotEmpty(path);

            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            if (String.IsNullOrEmpty(contents))
            {
                // Just to throw exception if there is a problem opening the file.
                new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read).Dispose();
                return Task.CompletedTask;
            }

            return WriteAllTextAsync(AsyncStreamWriter(path, encoding, append: true), contents, cancellationToken);
        }

        public static async Task WriteAllBytesAsync(String path, Byte[] bytes, CancellationToken cancellationToken = default)
        {
            PathNotEmpty(path);

            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            if (cancellationToken.IsCancellationRequested)
            {
                await Task.FromCanceled(cancellationToken).ConfigureAwait(false);
                return;
            }

            using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read,
                DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);

            await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);

            await stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }

        public static Task<Byte[]> ReadAllBytesAsync(String path, CancellationToken cancellationToken = default)
        {
            PathNotEmpty(path);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<byte[]>(cancellationToken);
            }

            var stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1, // bufferSize == 1 used to avoid unnecessary buffer in FileStream
                FileOptions.Asynchronous | FileOptions.SequentialScan);

            bool returningInternalTask = false;
            try
            {
                long fileLength = stream.Length;
                if (fileLength > int.MaxValue)
                {
                    var e = new IOException("SR.IO_FileTooLong2GB");

                    return Task.FromException<byte[]>(e);
                }

                returningInternalTask = true;

                return fileLength > 0 ?
                    ReadAllBytesAsync(stream, (int)fileLength, cancellationToken) :
                    ReadAllBytesUnknownLengthAsync(stream, cancellationToken);
            }
            finally
            {
                if (!returningInternalTask)
                {
                    stream.Dispose();
                }
            }
        }

        public static Task WriteAllTextAsync(String path, String contents, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = UTF8NoBOM;

            PathNotEmpty(path);

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            if (String.IsNullOrEmpty(contents))
            {
                new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read).Dispose();
                return Task.CompletedTask;
            }

            return WriteAllTextAsync(AsyncStreamWriter(path, encoding, append: false), contents, cancellationToken);
        }

        public static async Task<String> ReadAllTextAsync(String path, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            PathNotEmpty(path);

            if (cancellationToken.IsCancellationRequested)
                return await Task.FromCanceled<string>(cancellationToken).ConfigureAwait(false);

            char[] buffer = null;
            var reader = AsyncStreamReader(path, encoding);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                buffer = ArrayPool<char>.Shared.Rent(reader.CurrentEncoding.GetMaxCharCount(DefaultBufferSize));
                var sb = new StringBuilder();
                while (true)
                {
                    int read = await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (read == 0)
                    {
                        return sb.ToString();
                    }

                    sb.Append(buffer, 0, read);
                }
            }
            finally
            {
                reader.Dispose();
                if (buffer != null)
                {
                    ArrayPool<char>.Shared.Return(buffer);
                }
            }
        }

        public static async Task CopyToAsync(String path, Stream stream, CancellationToken cancellationToken = default)
        {
            using var file = new FileStream(path, FileMode.CreateNew);
            await stream.CopyToAsync(file, DefaultBufferSize, cancellationToken).ConfigureAwait(false);
        }

        private static StreamReader AsyncStreamReader(String path, Encoding encoding)
        {
            PathNotEmpty(path);
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));

            var stream = new FileStream(
                path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);

            return new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
        }

        private static StreamWriter AsyncStreamWriter(String path, Encoding encoding, Boolean append)
        {
            var stream = new FileStream(
                path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize,
                FileOptions.Asynchronous | FileOptions.SequentialScan);

            return new StreamWriter(stream, encoding);
        }

        private static async Task WriteAllLinesAsync(TextWriter writer, IEnumerable<String> contents, CancellationToken cancellationToken)
        {
            using (writer)
            {
                foreach (string line in contents)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await writer.WriteLineAsync(line).ConfigureAwait(false);
                }

                cancellationToken.ThrowIfCancellationRequested();
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        private static async Task WriteAllTextAsync(StreamWriter writer, String contents, CancellationToken cancellationToken)
        {
            char[] buffer = null;
            try
            {
                buffer = ArrayPool<char>.Shared.Rent(DefaultBufferSize);
                int count = contents.Length;
                int index = 0;
                while (index < count)
                {
                    int batchSize = Math.Min(DefaultBufferSize, count - index);
                    contents.CopyTo(index, buffer, 0, batchSize);
                    await writer.WriteAsync(buffer, 0, batchSize).ConfigureAwait(false);
                    index += batchSize;
                }

                cancellationToken.ThrowIfCancellationRequested();
                await writer.FlushAsync().ConfigureAwait(false);
            }
            finally
            {
                writer.Dispose();
                if (buffer != null)
                {
                    ArrayPool<char>.Shared.Return(buffer);
                }
            }
        }

        private static async Task<Byte[]> ReadAllBytesAsync(FileStream stream, int count, CancellationToken cancellationToken)
        {
            using (stream)
            {
                int index = 0;
                byte[] bytes = new byte[count];
                do
                {
                    int n = await stream.ReadAsync(bytes, index, count - index, cancellationToken).ConfigureAwait(false);

                    if (n == 0)
                    {
                        throw new EndOfStreamException("SR.IO_EOF_ReadBeyondEOF");
                    }

                    index += n;
                } while (index < count);

                return bytes;
            }
        }

        private static async Task<Byte[]> ReadAllBytesUnknownLengthAsync(FileStream stream, CancellationToken cancellationToken)
        {
            byte[] rentedArray = ArrayPool<byte>.Shared.Rent(512);
            try
            {
                int bytesRead = 0;
                while (true)
                {
                    if (bytesRead == rentedArray.Length)
                    {
                        uint newLength = (uint)rentedArray.Length * 2;
                        if (newLength > MaxByteArrayLength)
                        {
                            newLength = (uint)Math.Max(MaxByteArrayLength, rentedArray.Length + 1);
                        }

                        byte[] tmp = ArrayPool<byte>.Shared.Rent((int)newLength);
                        Buffer.BlockCopy(rentedArray, 0, tmp, 0, bytesRead);
                        ArrayPool<byte>.Shared.Return(rentedArray);
                        rentedArray = tmp;
                    }

                    int n = await stream.ReadAsync(rentedArray, bytesRead, rentedArray.Length - bytesRead, cancellationToken).ConfigureAwait(false);

                    if (n == 0)
                    {
                        return rentedArray.AsSpan(0, bytesRead).ToArray();
                    }
                    bytesRead += n;
                }
            }
            finally
            {
                stream.Dispose();
                ArrayPool<byte>.Shared.Return(rentedArray);
            }
        }

        private static void PathNotEmpty(String path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (path.Length == 0) throw new ArgumentException("path is empty", nameof(path));
        }

#endif
    }
}
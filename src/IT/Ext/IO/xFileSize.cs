using System;

namespace IT.Ext
{
    public static class xFileSize
    {
        /// <summary>
        /// Получить количество байт
        /// </summary>
        public static Int32 GetBytes(this FileSize size, Int32 count = 1) => (Int32)size * count;

        public static Int32 GetParts(this FileSize size, Int64 length, Int32 count = 1) => xFile.GetParts(length, size.GetBytes(count));
    }
}
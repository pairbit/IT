using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IT.Ext
{
    public static class xFile
    {
        private const Int32 FileNameMaxLength = 128;

        //TODO: Временный багфикс, чтобы не было проблем с StringTemplate из-за точки
        //private const String SysNamePattern = @"^[A-z0-9_\.]+$";

        private const String SysNamePattern = @"^[A-z0-9_]+$";

        private static readonly String[] ReservedNames = new string[]
        {
            "con","prn","aux","nul",
            "com0","com1","com2","com3","com4","com5","com6","com7","com8","com9",
            "lpt0","lpt1","lpt2","lpt3","lpt4","lpt5","lpt6","lpt7","lpt8","lpt9",
        };

        /// <summary>
        /// Получить имя файла без всех расширений: fileName.tar.gz
        /// </summary>
        /// <param name="fileName">fileName.tar.gz</param>
        /// <returns>fileName</returns>
        public static String GetFileNameWithoutExtensions(this String fileName)
        {
            if (fileName != null)
            {
                var index = fileName.IndexOf('.');
                if (index >= 0) return fileName.Remove(index);
            }
            return fileName;
        }

        /// <summary>
        /// Получить имя файла без одного расширения: fileName.tar.gz
        /// </summary>
        /// <param name="fileName">fileName.tar.gz</param>
        /// <returns>fileName.tar</returns>
        public static String GetFileNameWithoutExtension(this String fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        public static Boolean IsReservedFileName(this String name)
        {
            return ReservedNames.Contains(name.GetFileNameWithoutExtensions(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Проверить название файла на корректность
        /// </summary>
        /// <param name="value">Название файла</param>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
        /// </remarks>
        public static Boolean IsFileName(this String value)
        {
            return !value.IsSpace() &&
                    value.Trim().Equals(value) &&
                    value.Length <= FileNameMaxLength &&
                    value.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 && 
                   !value.EndsWith(".") &&
                   !value.IsReservedFileName();
        }

        /// <summary>
        /// Проверить системное название.
        /// Допускаются только циры, латинский алфавит и символы '.' и '_'
        /// </summary>
        public static Boolean IsSysName(this String value) => value.IsFileName() && Regex.IsMatch(value, SysNamePattern);

        /// <summary>
        /// Получить количество частей файла
        /// </summary>
        /// <param name="length">Общая длинная файла</param>
        /// <param name="count">Длинна части файла</param>
        /// <returns></returns>
        public static Int32 GetParts(Int64 length, Int32 count) => (Int32)Math.Ceiling(length / (Double)count);

        public static String GetSizeDisplay(Int64? size)
        {
            if (size == null) return null;
            var postfix = "Кб";
            var newSize = (decimal)size / 1024;

            if (newSize > 999.0m)
            {
                postfix = "Мб";
                newSize /= 1024;
                return string.Format("{0:N1} " + postfix, newSize);
            }

            if (newSize < 1)
            {
                newSize = 1;
            }
            return string.Format("{0:N0} " + postfix, newSize);
        }
    }
}
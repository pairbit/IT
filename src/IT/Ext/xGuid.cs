using System;
using System.IO;

namespace IT.Ext
{
    public static class xGuid
    {
        private static readonly String DirSep = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// Guid.ToString("N")
        /// </summary>
        public static String WithoutDash(this Guid value) => value.ToString("N");

        /// <summary>
        /// value.WithoutDash().Insert(6, DirSep).Insert(4, DirSep).Insert(2, DirSep)
        /// </summary>
        public static String WithDirSep(this Guid value) => value.WithoutDash().Insert(6, DirSep).Insert(4, DirSep).Insert(2, DirSep);
    }
}
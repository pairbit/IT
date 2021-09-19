using System;
using System.IO;

namespace IT.Ext
{
    public static class xDir
    {
        public static DirectoryInfo TryCreate(String path)
        {
            var dir = xPath.TryGetDir(path);
            if (dir == null || Directory.Exists(dir)) return null;
            return Directory.CreateDirectory(dir);
        }
    }
}
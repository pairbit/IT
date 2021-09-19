using IT.Validation;
using System;
using System.IO;
using System.Linq;

namespace IT.Ext
{
    public static class xPath
    {
        public static String TryGetDir(String path) => path.IsSpace() ? null : Path.GetDirectoryName(path);

        public static String Combine(params String[] paths) => Path.Combine(paths.UnSpace().ToArray());

        /// <exception cref="ArgumentNullException"/>
        public static String GetDir(String path)
        {
            var dir = TryGetDir(path);
            Arg.NotNull(dir);
            return dir;
        }
    }
}
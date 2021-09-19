using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IT.Ext
{
    public static class xAssembly
    {
        public static String GetVersion(this Assembly assembly)
        {
            var version = assembly.GetAttribute<AssemblyFileVersionAttribute>().Version;
            var v = new Version(version);
            return $"v{v.Major}.{v.Minor}.{v.Build}";
        }

        public static String GetLocationConfig(this Assembly assembly, String ext = ".json") => Path.Combine(Path.GetDirectoryName(assembly.Location), assembly.Name() + ext);

        public static String GetFullVersion(this Assembly assembly, String dateFormat = "dd.MM.yyyy HH:mm:ss")
        {
            var date = File.GetLastWriteTime(assembly.Location);
            return $"{assembly.GetVersion()} от {date.Tos(dateFormat)}";
        }

        public static String Name(this Assembly value) => value.FullName.Remove(value.FullName.IndexOf(','));

        public static String PublicKeyToken(this Assembly value) => GetInfo(value.FullName);

        public static String Version(this Assembly value) => GetInfo(value.FullName);

        public static String Culture(this Assembly value) => GetInfo(value.FullName);

        public static String ProcessorArchitecture(this Assembly value) => GetInfo(value.FullName);

        private static String GetInfo(String fullName, [CallerMemberName] String name = null)
        {
            name += "=";
            var start = fullName.IndexOf(name);
            if (start < 0) return null;
            start += name.Length;
            var end = fullName.IndexOf(',', start);
            if (end < 0) end = fullName.Length;
            return fullName.Substring(start, end);
        }
    }
}
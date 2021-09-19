using IT.Ext;
using System;
using System.IO;
using System.Reflection;

namespace IT
{
    public static class This
    {
        internal static readonly Assembly Assembly = typeof(This).Assembly;

        public static readonly Assembly EntryAssembly = Assembly.GetEntryAssembly();
        public static readonly String EntryAssemblyName = EntryAssembly.Name();
        public static readonly String ProjectName = EntryAssemblyName.Split('.')[0];

        public static readonly Func<Assembly> CallingAssembly = Assembly.GetCallingAssembly;
        public static readonly Func<Assembly> ExecutingAssembly = Assembly.GetExecutingAssembly;

        //AppContext.BaseDirectory
        //Environment.CurrentDirectory
        public static readonly string Dir = Environment.CurrentDirectory;
        public static readonly string Files = Path.Combine(Dir, nameof(Files)) + Path.DirectorySeparatorChar;
        public static readonly Func<MethodBase> Method = MethodBase.GetCurrentMethod;

        public static String GetFile(String fileName, params String[] path)
        {
            return Path.Combine(Files, Path.Combine(path), fileName);
        }
    }
}
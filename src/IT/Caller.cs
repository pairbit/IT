using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace IT
{
    public static class Caller
    {
        public static String GetTypeNameFromFilePath([CallerFilePath] String filePath = null) => Path.GetFileNameWithoutExtension(filePath);
    }
}
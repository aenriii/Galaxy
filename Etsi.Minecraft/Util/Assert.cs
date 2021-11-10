using System;
using System.IO;
using System.Net.Sockets;

namespace Etsi.Minecraft.Util
{
    public static class Assert
    {
        public static bool FileExists(string FilePath)
        {
            return File.Exists(FilePath);
        }
        public static bool FolderExists(string FolderPath)
        {
            return Directory.Exists(FolderPath);
        }

        public static string GetFaviconPath()
        {
            if (FileExists("./favicon.ico"))
                return "./favicon.ico";
            return String.Empty;
        }
        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("Assertion failed, " + message);
            }
        }
        public static bool StreamHasData(NetworkStream stream)
        {
            return stream.DataAvailable;
        }
    }
}
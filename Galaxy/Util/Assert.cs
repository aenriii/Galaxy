using System;
using System.IO;

namespace Galaxy.Util
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
    }
}
using System.IO;
using System;

using Galaxy.Internal;

namespace Galaxy
{
    public class StaticMount : IGalaxyWare
    {
        public StaticMount(string folderPath, string routeMount, bool recursive = true)
        {
            if (!File.Exists(folderPath))
            {
                throw new FileNotFoundException("folderPath not exists");
            }
        }
        public bool HandleRequest(HttpContext context)
        {
            return true;
        }
    }
}
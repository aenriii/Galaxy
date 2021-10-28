using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Galaxy.Internal;
using System.Text;
namespace Galaxy
{
    public class StaticMount : IGalaxyWare
    {
        internal string folderPath;
        internal string routeMount;
        internal bool recursive;
        internal List<string> routeMountPathList;
        public StaticMount(string folderPath, string routeMount, bool recursive = true)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new FileNotFoundException("folderPath not exists");
            }
            this.routeMount = routeMount;
            this.folderPath = folderPath;
            this.recursive = recursive;
            this.routeMountPathList =  = this.routeMount.Split("/").Where<string>(x => x != string.Empty);
        }
        public bool HandleRequest(HttpContext context)
        {
            if (context.Request.HttpMethod.ToLower() != "get")
                return false;
            List<string> path = context.Request.Url.AbsolutePath.Split("/").Where<string>(x => x != string.Empty).ToList<string>();
            if(this.routeMountPathList.Count() > 1)
                throw new NotImplementedException("Subfolder Mounts are not implemented yet.");
            if (path[0] != routeMountPathList[0])
                return false;
            if (path.Where<string>(x => x.Trim() == "..").Count() != 0)
            {
                context.Response.StatusCode = 400;
                context.WriteString("Disallowed file path \"..\"");
                context.Response.Close();
                return true;
            }
            
            try
            {
                string p = Path.GetRelativePath(Directory.GetCurrentDirectory(), Join("/", path));
                if (new Uri(Directory.GetCurrentDirectory()).IsBaseOf(new Uri(p)))
                {
                    
                }

            }
            catch (Exception e)
            {

            }
            finally
            {

            }

            return false;
        }
        internal string Join(string sep, List<string> l)
        {
            
            StringBuilder str = new StringBuilder();
            bool t = false;
            foreach(string s in l)
            {
                if (!t)
                {
                    str.Append(s);
                    t = true;
                    continue;
                }
                str.Append(s);
                str.Append("/");
            }
            str.Remove(str.Length - 1, 1); // removes the last / from the string
            return str.ToString();
        }
    }
}
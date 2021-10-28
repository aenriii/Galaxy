using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Web;

namespace Galaxy
{
    public class HttpContext
    {
        public HttpListenerRequest Request;
        public HttpListenerResponse Response;
        public HttpContext(HttpListenerContext lctx)
        {
            this.Request = lctx.Request;
            this.Response = lctx.Response;
        }
        public async Task StreamResponse(byte[] res)
        {
            throw new NotImplementedException("Not Implemented");
            return;
        }
        public void WriteString(string res)
        {
            Response.OutputStream.Write(Encoding.UTF32.GetBytes(res));
        }
        public void SendFile(string FilePath)
        {
            try
            {
                Response.ContentType = MimeMapping.MimeUtility.GetMimeMapping(FilePath);
                Response.StatusCode = 200;
                Response.OutputStream.Write(File.ReadAllBytes(FilePath));
            }
            catch (Exception e)
            {
                Response.ContentType = MimeMapping.KnownMimeTypes.Text;
                Response.StatusCode = 404;
                WriteString("404 not found " + Request.RawUrl);
            }
            finally
            {
                Response.Close();
            }
        }
    }
}
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Galaxy.Util;
using MimeMapping;
using Newtonsoft.Json;

namespace Galaxy.Internal
{
    public class HttpContext
    {
        public HttpMethod Method;
        public HttpListenerRequest Request;
        public HttpListenerResponse Response;
        public bool Open;
        
        public HttpContext(HttpListenerContext context)
        {
            Request = context.Request;
            Response = context.Response;
            Method = new (Request.HttpMethod);
            Open = true;
        }
        public void Close()
        {
            if (Open)
                Response.Close();
            Open = false;
        }

        #region File Write
        
        public bool TryWriteFile(string FilePath)
        {
            if (!Open)
                return false;
            if (Assert.FileExists(FilePath))
            {
                Response.ContentType = MimeMapping.MimeUtility.GetMimeMapping(FilePath);
                Response.ContentLength64 = new FileInfo(FilePath).Length;
                using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(Response.OutputStream);
                }
                return true;
            }
            return false;
        }
        public bool TryWriteFileAt(string FilePath, long Offset)
        {
            if (!Open)
                return false;
            if (Assert.FileExists(FilePath))
            {
                Response.ContentType = MimeMapping.MimeUtility.GetMimeMapping(FilePath);
                Response.ContentLength64 = new FileInfo(FilePath).Length;
                using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    fs.Seek(Offset, SeekOrigin.Begin);
                    fs.CopyTo(Response.OutputStream);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Stream Write

        public bool TryWriteStream(Stream Stream)
        {
            if (!Open)
                return false;
            if (Stream != null)
            {
                // Assumes Mime-Type is set
                Stream.CopyTo(Response.OutputStream);
                return true;
            }
            return false;
        }
        public bool TryWriteStream(Stream Stream, string MimeType)
        {
            if (!Open)
                return false;
            if (Stream != null)
            {
                Response.ContentType = MimeType;
                Stream.CopyTo(Response.OutputStream);
                return true;
            }
            return false;
        }
        public bool TryWriteStream(Stream Stream, string MimeType, long ContentLength)
        {
            if (!Open)
                return false;
            if (Stream != null)
            {
                Response.ContentType = MimeType;
                Response.ContentLength64 = ContentLength;
                Stream.CopyTo(Response.OutputStream);
                return true;
            }
            return false;
        }
        public bool TryWriteStream(Stream stream, long Offset)
        {
            if (!Open)
                return false;
            if (stream != null)
            {
                stream.Seek(Offset, SeekOrigin.Begin);
                stream.CopyTo(Response.OutputStream);
                return true;
            }
            return false;
        }

        #endregion
        
        #region Buffer Write
        
        public bool TryWriteBuffer(byte[] Buffer)
        {
            if (!Open)
                return false;
            if (Buffer != null)
            {
                // Assumes Mime-Type is set
                try
                {
                    Response.OutputStream.Write(Buffer, 0, Buffer.Length);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        
        #endregion

        #region Set Response Headers
        public void SetMimeType(string FileName)
        {
            Response.ContentType = MimeUtility.GetMimeMapping(FileName);
        }
        public void SetContentLength(long ContentLength)
        {
            Response.ContentLength64 = ContentLength;
        }

        public void SetContentType(string MimeType)
        {
            Response.ContentType = MimeType;
        }
        #endregion

        #region Write JSON

        public bool TryWriteJson(object Object)
        {
            if (!Open)
                return false;
            Response.ContentType = "application/json";
            Response.ContentEncoding = Encoding.UTF8;
            if(TryWriteString(JsonConvert.SerializeObject(Object)))
                return true;
            return false;
        }


        #endregion

        #region Write String
        public bool TryWriteString(string String)
        {
            if (!Open)
                return false;
            // Assumes Mime-Type is set
            Response.ContentEncoding = Encoding.UTF8;
            if (TryWriteBuffer(
                // get byte[] from string
                Encoding.UTF8.GetBytes(String)
                ))
                return true;
            return false;
        }

        #endregion
        
        #region Write Error
        public bool TryWriteError(int StatusCode)
        {
            if (!Open)
                return false;
            Response.StatusCode = StatusCode;
            return TryWriteString("Error " + StatusCode);
        }
        #endregion
    }
}
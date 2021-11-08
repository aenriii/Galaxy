using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using Galaxy;
using Galaxy.Http;

namespace GalaxyExampleServer
{
    class Program
    {
        private static GalaxyServer Server;
        public static void Main(string[] args)
        {
            Server = new GalaxyServer(4000); 
            // use default configuration
            // which listens on port 4000
            // and replys with a "hello world"
            // page
            Server.ServeStaticFolder(Path.Join(Path.GetPathRoot("/"), "srv", "www", "assets"), "/assets"); 
            Server.Get("/", ctx =>
            {
                if (!ctx.TryWriteFile(Path.Join(Path.GetPathRoot("/"), "srv", "www", "index.html")))
                {
                    ctx.TryWriteError(500);
                    ctx.Close();
                }
                
            });


            Server.Start(); // This is the function to start the server. The
                            // ASP.NET Core equivalent to this function is
                            // "blocking", which means it prevents the next
                            // instruction from running. This function,
                            // however, is "non-blocking". This means that
                            // the next instructions will run even though
                            // there are still things being done with this
                            // instruction. This allows for multiple services
                            // to be run in concurrency from one application
                            // which makes "IPC" (or Inter-Program Communication)
                            // infinitely easier.
            Console.WriteLine("Running Galaxy Server"); // This would not run in ASP.NET Core because it's Server
                                                        // Start function is "blocking", however it runs here 
                                                        // because the GalaxyServer Start function is 
                                                        // "non-blocking"
            HttpResponseMessage res = new HttpClient().GetAsync("http://localhost:4000/assets/image.png").Result;
            // assert that it's equal to the actual file
            if (res.Content.ReadAsByteArrayAsync().Result == File.ReadAllBytes("www/assets/image.png"))
            {
                System.Console.WriteLine("Assertion passed, content valid");
            }
            else
            {
                System.Console.WriteLine("Assertion failed, content not the same.");
            }
        }

        
    }
}
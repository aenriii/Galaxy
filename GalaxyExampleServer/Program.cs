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
        private static Dictionary<HttpRouteDetails, Action<HttpListenerContext>> Routes = new Dictionary<HttpRouteDetails, Action<HttpListenerContext>>();
        public static void Main(string[] args)
        {
            GetRoutes();
            Server = new GalaxyServer(new GalaxyServerConfiguration(4000, Routes)); 
            // use default configuration
            // which listens on port 4000
            // and replys with a "hello world"
            // page
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
        }

        private static void GetRoutes()
        {
            Routes.Add(
                new HttpRouteDetails(HttpMethod.Get, "/"),
                new Action<HttpListenerContext>(
                    ctx =>
                    {
                        try
                        {
                            byte[] index = File.ReadAllBytes("www/index.html");
                            ctx.Response.StatusCode = 200;

                            ctx.Response.OutputStream.Write(index);
                            ctx.Response.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            ctx.Response.StatusCode = 500;
                            ctx.Response.OutputStream.Write(Encoding.ASCII.GetBytes("<h1>500 INTERNAL SERVER ERROR</h1><p>FileReadError</p>"));
                            ctx.Response.Close();
                        }
                    }
                )
            );
            Routes.Add(
                new HttpRouteDetails(HttpMethod.Get, "/assets/image.png"),
                new Action<HttpListenerContext>(
                    ctx =>
                    {
                        try
                        {
                            byte[] index = File.ReadAllBytes("www/assets/image.png");
                            ctx.Response.StatusCode = 200;
                            ctx.Response.AddHeader("Content-Type", "image/png");
                            ctx.Response.OutputStream.Write(index);
                            ctx.Response.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            ctx.Response.StatusCode = 500;
                            ctx.Response.OutputStream.Write(Encoding.ASCII.GetBytes("<h1>500 INTERNAL SERVER ERROR</h1><p>FileReadError</p>"));
                            ctx.Response.Close();
                        }
                    }
                )
            );
        }
    }
}
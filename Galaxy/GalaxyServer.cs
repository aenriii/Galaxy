
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Galaxy.Http;
using System.Linq;
using System.Text;
using System.Threading;


namespace Galaxy
{
    #nullable enable
    public class GalaxyServer
    {
        internal HttpListener Listener;
        internal GalaxyServerConfiguration Configuration;
        internal Dictionary<string, Action<HttpListenerContext>> GETRoutes = new Dictionary<string, Action<HttpListenerContext>>();
        internal Dictionary<string, Action<HttpListenerContext>> POSTRoutes = new Dictionary<string, Action<HttpListenerContext>>();
        internal Dictionary<string, Action<HttpListenerContext>> PUTRoutes = new Dictionary<string, Action<HttpListenerContext>>();
        internal Dictionary<string, Action<HttpListenerContext>> DELETERoutes = new Dictionary<string, Action<HttpListenerContext>>();
        
        
        

        public GalaxyServer(GalaxyServerConfiguration? config)
        {
            Configuration = config ?? default(GalaxyServerConfiguration);
            foreach (KeyValuePair<HttpRouteDetails, Action<HttpListenerContext>> route in Configuration.Routes)
            {
                
                    if     (route.Key.Method == HttpMethod.Get)
                        GETRoutes.Add   (route.Key.Route, route.Value);
                    else if(route.Key.Method == HttpMethod.Post)
                        POSTRoutes.Add  (route.Key.Route, route.Value);
                    else if(route.Key.Method == HttpMethod.Get)
                        PUTRoutes.Add   (route.Key.Route, route.Value);
                    else if(route.Key.Method == HttpMethod.Get)
                        DELETERoutes.Add(route.Key.Route, route.Value);
                    else
                        Console.Write(new ArgumentException("Method not permitted currently: " + route.Key.Method.ToString()));
            }

            Listener = new HttpListener();
            if (Configuration.Ports != null)
                foreach (int port in Configuration.Ports)
                    AddPort(port);
            else
                AddPort(Configuration.Port.Value); // ! asserting that C.Port is non-null, as it most definitely is if C.Ports isnt
            

        }
        
        /*
         * Internal Methods
         */
        internal void AddPort(int port)
        {
            if (this.Listener == null)
            {
                throw new ArgumentNullException("Listener is not initialized.");
            }
            Listener.Prefixes.Add($"http{(Configuration.UseHttps ? "s" : "")}://*:{port}/");
        }

        internal void HandleConnections()
        {
            bool run = true;
            while (run)
            {
                HttpListenerContext ctx = Listener.GetContextAsync().Result;
                HttpMethod m = new HttpMethod(ctx.Request.HttpMethod);
                Action<HttpListenerContext> act;
                Thread t;
                bool processed = false;
                if (m == HttpMethod.Get)
                {
                    foreach (KeyValuePair<string, Action<HttpListenerContext>> r in GETRoutes)
                    {
                        if (ctx.Request.Url.AbsolutePath == r.Key)
                        {
                            t = new Thread(
                                () => { r.Value(ctx); });
                            t.Start();
                            processed = true;
                            break;
                        }

                    }

                    if (!processed)
                    {
                        ctx.Response.StatusCode = 404;
                        ctx.Response.OutputStream.Write(Encoding.ASCII.GetBytes("" +
                            "<html>" +
                            "<head>" +
                            "<title> 404 </title>" +
                            "</head>" +
                            "<body>" +
                            "<h1>" +
                            "Method unacceptable" +
                            "</h1>" +
                            "<p>" +
                            ctx.Request.Url.AbsolutePath + " Not Found" +
                            "</p>" +
                            "</body>" +
                            "</html>"));
                    }
                }
                else
                {
                    ctx.Response.StatusCode = 400;
                    ctx.Response.OutputStream.Write(Encoding.ASCII.GetBytes("" +
                                                                            "<html>" +
                                                                            "<head>" +
                                                                            "<title> 400 </title>" +
                                                                            "</head>" +
                                                                            "<body>" +
                                                                            "<h1>" +
                                                                            "Method unacceptable" +
                                                                            "</h1>" +
                                                                            "<p>" +
                                                                            ctx.Request.HttpMethod + " Not Implemented Yet" +
                                                                            "</p>" +
                                                                            "</body>" +
                                                                            "</html>"));
                }

            }
        }
        
        /*
         * Public Methods
         */

        public void Start()
        {
            Listener.Start();
            new Thread( this.HandleConnections ).Start();
        }
        
    }
}
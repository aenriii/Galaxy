
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using Galaxy.Http;
using System.Linq;
using System.Text;
using System.Threading;
using Galaxy.Internal;
using Galaxy.Util;

namespace Galaxy
{
    #nullable enable
    public class GalaxyServer : IGalaxyServer
    {
        /*
         * Complete remake.
         */
        private ListDictionary<IGalaxyEvent, GalaxyRoute> EventPools;
    
        private Dictionary<HttpMethod, IGalaxyEvent> Events;
        private HttpListener Listener;
        private Thread ListenerThread;
        private bool IsRunning;
        private bool IsStopped;
        private List<Action<HttpContext>> WaresAction;
        public GalaxyServer(int port)
        {
            EventPools = new ListDictionary<IGalaxyEvent, GalaxyRoute>();
            Events = new Dictionary<HttpMethod, IGalaxyEvent>();
            IsRunning = false;
            IsStopped = false;
            Listener = new();
            WaresAction = new List<Action<HttpContext>>();
            AddPort(port);
        }

        #region Public Methods

        public void UseMiddleware(IGalaxyWare ware)
        {
            throw new NotImplementedException();
        }
        public void UseMiddleware(Action<HttpContext> ware)
        {
            this.WaresAction.Add(ware);
        }

        

        public void RemoveMiddleware(IGalaxyWare ware)
        {
            throw new NotImplementedException();
        }

        public void Get(string route, Action<HttpContext> action)
        {
            
            if (EventPools.TryGetValue(IGalaxyEvent.Get, out var list))
            {
                if (list.Find(x =>  x.Matches(route)) != null)
                {
                    throw new ArgumentException("Route already exists (" + route + ")");
                }
            }
            if (!this.EventPools.TryAdd(
                IGalaxyEvent.Get,

                new GalaxyRoute(
                    new HttpRouteDetails(HttpMethod.Get, route),
                    action
                )
            ))
            {
                throw new AggregateException("Adding " + route + " to the EventPool for 'GET' failed.");
            }
        }

        public void Post(string route, Action<HttpContext> action)
        {
            if (EventPools.TryGetValue(IGalaxyEvent.Post, out var list))
            {
                if (list.Find(x =>  x.Matches(route)) != null)
                {
                    throw new ArgumentException("Route already exists (" + route + ")");
                }
            }
            if (!this.EventPools.TryAdd(
                IGalaxyEvent.Post,

                new GalaxyRoute(
                    new HttpRouteDetails(HttpMethod.Get, route),
                    action
                )
            ))
            {
                throw new AggregateException("Adding " + route + " to the EventPool for 'POST' failed.");
            }
        }

        // public void Head(string route, Action<HttpContext> action)
        // {
        //     // TODO: Make fake Response and Request objects to treat as a GET request as per HTTP spec.
        //     throw new NotImplementedException();
        // }
        //
        // public void Options(string route, Action<HttpContext> action)
        // {
        //     // TODO: Dynamically generate a response object to return.
        //     throw new NotImplementedException();
        // }

        public void Delete(string route, Action<HttpContext> action)
        {
            if (EventPools.TryGetValue(IGalaxyEvent.Delete, out var list))
            {
                if (list.Find(x =>  x.Matches(route)) != null)
                {
                    throw new ArgumentException("Route already exists (" + route + ")");
                }
            }
            if (!this.EventPools.TryAdd(
                IGalaxyEvent.Delete,

                new GalaxyRoute(
                    new HttpRouteDetails(HttpMethod.Get, route),
                    action
                )
            ))
            {
                throw new AggregateException("Adding " + route + " to the EventPool for 'DELETE' failed.");
            }
        }

        public void Put(string route, Action<HttpContext> action)
        {
            if (EventPools.TryGetValue(IGalaxyEvent.Put, out var list))
            {
                if (list.Find(x =>  x.Matches(route)) != null)
                {
                    throw new ArgumentException("Route already exists (" + route + ")");
                }
            }
            if (!this.EventPools.TryAdd(
                IGalaxyEvent.Put,

                new GalaxyRoute(
                    new HttpRouteDetails(HttpMethod.Get, route),
                    action
                )
            ))
            {
                throw new AggregateException("Adding " + route + " to the EventPool for 'PUT' failed.");
            }
        }

        public void ServeStaticFile(string path, string route, string? CORS = null)
        {
            if (!Assert.FileExists(path))
            {
                throw new ArgumentException("File does not exist", nameof(path));
            }
            if (EventPools.TryGetValue(IGalaxyEvent.Get, out var list))
            {
                if (list.Find(x =>  x.Matches(route)) != null)
                {
                    throw new ArgumentException("Route already exists (" + route + ")");
                }
            }
            if (!this.EventPools.TryAdd(
                IGalaxyEvent.Get,

                new GalaxyRoute(
                    new HttpRouteDetails(HttpMethod.Get, route),
                    (context =>
                    {
                        Console.WriteLine("Serving static file: " + path);
                        context.Response.AddHeader("Access-Control-Allow-Origin", CORS ?? "*");
                        context.TryWriteFile(path);
                    })
                )
            ))
            {
                throw new AggregateException("Adding " + route + " to the EventPool for 'GET' failed.");
            }
        }

        public void ServeStaticFolder(string path, string route)
        {
            if(!Assert.FolderExists(path))
                throw new ArgumentException("Folder does not exist", nameof(path));
            // iterate over files in folder and add them to the event pool
            foreach (var file in Directory.GetFiles(path))
            {
                Console.WriteLine("Adding " + file + " to the EventPool for 'GET'");
                if (Directory.Exists(route + "/" + Path.GetFileName(file)))
                {
                    ServeStaticFolder(file,route + "/" + Path.GetFileName(file)        );
                }
                ServeStaticFile(file, route + "/" + Path.GetFileName(file));
            }
        }

        public void StartAsDevelopment(int port)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            IsRunning = true;
            Listen();
        }

        #endregion

        #region Private Methods

        private void BeginEventDispatch()
        {
            while (IsRunning)
            {
                if(!Listener.IsListening)
                    continue;
                var context = Listener.GetContext();
                
                HttpContext httpContext = new HttpContext(context);
                var dispatchThread = new Thread(
                    () => DispatchResponse(httpContext));
                dispatchThread.Start();


            }
        }

        private void DispatchResponse(HttpContext httpContext)
        {
            Console.WriteLine("Received request: " + httpContext.Request.Url);
            foreach (Action<HttpContext> action in this.WaresAction)
            {
                if (httpContext.Open)
                    action(httpContext);
                else return;
            }
            
            // get the event pool for the request
            if (EventPools.TryGetValue(new IGalaxyEvent(httpContext.Method.Method), out var list))
            {
                
                // find the route
                var route = list.Find(x => x.Matches(httpContext.Request.Url.AbsolutePath.DecodeUriComponents()));
                if (route != null)
                {
                    // execute the route
                    route.Handle(httpContext);
                }
                else
                {
                    // no route found
                    httpContext.TryWriteError(404);
                    
                    // if the route is favicon.ico, write the default favicon
                    if (httpContext.Request.Url.AbsolutePath == "/favicon.ico")
                    {
                        httpContext.TryWriteFile(Assert.GetFaviconPath());
                    }
                }
            }
            else
            {
                // no event pool found
                httpContext.TryWriteError(404);
            }
        }
        void Listen()
        {
            Listener.Start();
            ListenerThread = new Thread(BeginEventDispatch);
            ListenerThread.Start();
        }

        void AddPort(int port)
        {
            // add port to the list of prefixes
            Listener.Prefixes.Add(
                "http://*:" + port + "/"
                );
        }

        #endregion
    }
    
}
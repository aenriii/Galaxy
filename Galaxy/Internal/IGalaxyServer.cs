using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Galaxy.Internal
{
    public interface IGalaxyServer
    {
        #region Attributes

        // private Dictionary<IGalaxyEvent, EventPool<Action<HttpContext>>> EventPools;
        //
        // private Dictionary<HttpMethod, IGalaxyEvent> Events;
        // private HttpListener Listener;

        #endregion
        
        
        #region External Functions
        
        // constructor not included //
        public void UseMiddleware(IGalaxyWare ware);
        public void RemoveMiddleware(IGalaxyWare ware);
        public void Get(string route, Action<HttpContext> action);
        public void Post(string route, Action<HttpContext> action);
        // public void Head(string route, Action<HttpContext> action);
        // public void Options(string route, Action<HttpContext> action);
        public void Delete(string route, Action<HttpContext> action);
        public void Put(string route, Action<HttpContext> action);

        public void ServeStaticFile(string path, string route);
        public void ServeStaticFolder(string path, string route);
        // TODO: Templated Sites
        // public void ServeTemplatedSite(string path, string route, IGalaxyTemplateConfiguration templateConfiguration);
        
        // TODO: Multi-Port Support
        
        public void StartAsDevelopment(int port);
        // public void StartAsDevelopment(int[] ports); 
        // public void Start(int port);
        // public void Start(int[] ports);
        public void Start();

        #endregion
        
        #region Internal Functions

        // internal void BeginEventDispatch();
        // internal void Listen();
        // internal void AddPort(int port);

        #endregion


    }
}

using System;
using System.Net.Http;
using Galaxy.Http;
using Galaxy.Internal;


namespace Galaxy
{
    public class GalaxyRoute
    {
        public HttpRouteDetails Route;
        public Action<HttpContext> Handle;

        public static GalaxyRoute GetInstance(HttpRouteDetails hrd, Action<HttpContext> ahc)
        {
            return new GalaxyRoute(hrd, ahc);
        }

        public GalaxyRoute(HttpRouteDetails hrd, Action<HttpContext> ahc)
        {
            this.Route = hrd ?? new HttpRouteDetails(HttpMethod.Get);
            this.Handle = ahc ?? (context =>
            {
                context.Response.StatusCode = 500;
                if (!context.TryWriteString("500 route not accessible"))
                    context.TryWriteString("500 internal server error");

                    context.Response.Close();
            });
        }
        public bool Matches(HttpRequestMessage req)
        {
            // If route matches request url path
            // and method matches request method

            try
            {
                if (req.RequestUri.AbsolutePath == Route.Route &&
                    (Route.Method == null || req.Method == Route.Method))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }public bool Matches(string req)
        {
            // If route matches request url path

            try
            {
                if (new Uri(req).AbsolutePath == Route.Route)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }
    }
}
using System.Net.Http;

namespace Galaxy.Http
{
    #nullable enable
    public class HttpRouteDetails
    {
        public HttpMethod Method;
        public string Route;

        public HttpRouteDetails(HttpMethod? method , string route = "/")
        {
            this.Method = method ?? HttpMethod.Get;
            this.Route = route;
        }
    }
}
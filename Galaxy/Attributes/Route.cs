using System;
using System.Net.Http;

namespace Galaxy.Attributes
{
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Delegate |
        AttributeTargets.Method
    )]
    public class Route : GalaxyAttribute
    {
        public string route;
        public HttpMethod? method;
        public Route(string route, HttpMethod? method = null)
        {
            this.route = route;
            this.method = method ?? HttpMethod.Get;
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Galaxy.Http;
namespace Galaxy
{
    #nullable enable
    public class GalaxyServerConfiguration
    {
        public Dictionary<HttpRouteDetails, Action<HttpListenerContext>> Routes;
        public int? Port;
        public List<int>? Ports;
        public bool UseHttps;

        public GalaxyServerConfiguration(
            Dictionary<HttpRouteDetails, Action<HttpListenerContext>>? routes,
            int? port,
            List<int>? ports,
            bool? https
        )
        {
            Routes = routes ?? new Dictionary<HttpRouteDetails, Action<HttpListenerContext>>()
            {
                {
                    new HttpRouteDetails(null, "/"), context =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.OutputStream.Write(Encoding.ASCII.GetBytes("" +
                            "<html>" +
                            "<head>" +
                            "<title> Hello World </title>" +
                            "</head>" +
                            "<body>" +
                            "<h1>" +
                            "This is the default page for the Galaxy Server." +
                            "</h1>" +
                            "<p>" +
                            "Try setting some routes!" +
                            "</p>" +
                            "</body>" +
                            "</html>"));
                        context.Response.Close();
                    }
                }
            };
            if (port == null && ports == null)
            {
                Port = 4000;
            }

            if (https == null)
            {
                UseHttps = false;
            }

            if (https.Value)
            {
                throw new NotImplementedException("Https support not implemented (i think)");
            }
            
            
            
        ;

    }

        public GalaxyServerConfiguration(
            int port
        )
        {
            this.Port = port;
            this.Ports = null;
            this.UseHttps = false;
            this.Routes = new Dictionary<HttpRouteDetails, Action<HttpListenerContext>>()
            {
                {
                    new HttpRouteDetails(null, "/"), context =>
                    {
                        context.Response.StatusCode = 200;
                        context.Response.OutputStream.Write(Encoding.ASCII.GetBytes("" +
                            "<html>" +
                            "<head>" +
                            "<title> Hello World </title>" +
                            "</head>" +
                            "<body>" +
                            "<h1>" +
                            "This is the default page for the Galaxy Server." +
                            "</h1>" +
                            "<p>" +
                            "Try setting some routes!" +
                            "</p>" +
                            "</body>" +
                            "</html>"));
                        context.Response.Close();
                    }
                }
            };
        }
        public GalaxyServerConfiguration(
            int port,
            Dictionary<HttpRouteDetails, Action<HttpListenerContext>> routes)
        {
            this.Port = port;
            this.Ports = null;
            this.UseHttps = false;
            this.Routes = routes;
        }

    }
}
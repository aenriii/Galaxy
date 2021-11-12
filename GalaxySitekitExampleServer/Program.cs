using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Galaxy;
using Galaxy.Internal;
using Galaxy.Sitekit.Elements;
using Galaxy.Util;
using Galaxy.Internal;

namespace GalaxySitekitExampleServer
{
    class Program
    {
        static List<string> assets = new List<string>(){"main.css"};
        
        static void Main(string[] args)
        {
            GalaxyServer server = new GalaxyServer(
                8080
            );
            server.UseMiddleware(RequestDynamo);
            server.Post("/login", context =>
            {
                // process login from the body as username and password in query string format
                if (!context.Request.HasEntityBody)
                {
                    context.TryWriteError(400);
                    return;
                }
                char[] body = new char[300];
                int? length = null;
                for (var i = 0; i < 300; i++)
                {
                    try
                    {
                        byte[] b = new byte[1];
                        if (0 == context.Request.InputStream.Read(b, 0, 1))
                            break;
                        body[i] = (char) b[0];
                        length = i;
                        
                    }
                    catch (Exception)
                    {
                        break;
                    }

                }

                char[] _ = new char[(length ?? -1) + 1];
                for (var i = 0; i < _.Length; i++)
                {
                    _[i] = body[i];
                }
                
                body = _;


                // Get the encoding of the request from the headers
                string encoding = context.Request.Headers["Content-Encoding"];
                // Get the correct string by encoding
                
                string bodyString = String.Join("", body);
                
                
                // pass this off to a function that will parse the body and return a username and password
                string username = "";
                string password = "";
                ParseBody(bodyString, out username, out password);
                // check the username and password against the database
                if (username == "admin" && password == "admin")
                {
                    // if the username and password are correct, set the cookie and return a 200
                    context.Response.Cookies.Add(new Cookie("isAuthenticated", "true"));
                    context.TryWriteError(200);
                }
                else
                {
                    // if the username and password are incorrect, return a 401
                    context.TryWriteError(401);
                }
                

            });
            server.Start();
            
            
        }
        public static void RequestDynamo(HttpContext context)
        {
            // Console.WriteLine("ENTER requestdynamo, " + context.Request.Url.AbsolutePath);
            if (context.Method != HttpMethod.Get)
                return;
            // if not authenticated, send login page
            if (!(context.Request.Cookies.Where(x => x.Name == "isAuthenticated").Count() != 0) || context.Request.Cookies["isAuthenticated"].Value != "true")
            {
                // Console.WriteLine("sending login page");
                context.Response.ContentType = "text/html";
                BuildLoginPage(context);
                return;
            }
            string path = ParsePath(context.Request.Url);
            if (path.IndexOf(".backup") != -1 && context.Request.QueryString["bypass"]?.ToLower() != "true")
            {
                // Console.WriteLine(".backup detected, eliminating");
                context.TryWriteError(500);
            }
            // if (assets.Contains(path.Substring(path.LastIndexOf('/') + 1)))
            // {
            //     context.TryWriteFile(Path.Join(Directory.GetCurrentDirectory(), path));
            //     return;
            // }

            if (Assert.FileExists(path))
                if (context.TryWriteFile(path))
                    return;
                // else
                    // context.TryWriteError(500);
            if (Assert.FolderExists(path))
            {
                BuildSite(context, path);
                return;
            }

            if (path.Length < 5)
            {
                BuildSite(context, Path.GetPathRoot("./"));
            }
            // Console.WriteLine("throwing 404, not found " + path);
            context.TryWriteError(404);
            
        }

        private static void BuildLoginPage(HttpContext context)
        {
            // Console.WriteLine("Building Login page");
            string page = e.html(text:
                              e.head(text: (
                                      e.title(text: "Login") +
                                      // e.link(props: new Dictionary<string, string>()
                                      //     {{"rel", "stylesheet"}, {"href", "main.css"}}) +
                                      e.link(props: new Dictionary<string, string>()
                                      {
                                          {"rel", "stylesheet"},
                                          {
                                              "href",
                                              "https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
                                          }
                                      }) //+
                                      // e.script(props: new Dictionary<string, string>() {{"src", "main.js"}})
                              )) +
                          e.body(text: (
                              e.h1(text: "Login") +
                              e.form(props: new Dictionary<string, string>() {{"action", "/login"}, {"method", "POST"}}, text: (
                                  e.div(classes: new []{"form-group"}, text: (
                                      e.label(props: new Dictionary<string, string>() {{"for", "username"}}, text: "Username") +
                                      e.input(id: "username", props: new Dictionary<string, string>() {{"placeholder", "Enter username"}, {"name", "username"}}, classes: new []{"form-control"})
                                  )) +
                                  e.div(classes: new []{"form-group"}, text: (
                                      e.label(props: new Dictionary<string, string>() {{"for", "password"}}, text: "Password") +
                                      e.input(id: "password", props: new Dictionary<string, string>() {{"placeholder", "Password"}, {"name", "password"}}, classes: new []{"form-control"})
                                  )) + 
                                  e.button(props: new Dictionary<string, string>() {{"type", "submit"}}, text: "Submit", classes: new []{"btn", "btn-primary"})
                              ))
                              
                          ))
                );
            context.SetContentType("text/html");
            context.TryWriteString(page);
            // Console.WriteLine("wrote and closing login page");
            if (context.Open)
                context.Close();
        }

        public static string ParsePath(Uri url)
        {
            string p = url.AbsolutePath.DecodeUriComponents();
            if (p == string.Empty || p is null)
                return Path.GetPathRoot("/");
            return p;

        }

        public static void ParseBody(string body, out string un, out string pw)
        {
            // Console.WriteLine("parsing body " + body);
            // parse the url-encoded form data
            string[] parts = body.Split('&');
            un = parts[0].Split('=')[1];
            pw = parts[1].Split('=')[1];

        }
        

        public static void BuildSite(HttpContext context, string path)
        {
            // if file is an asset, provide that asset
            // safe to assume that the path is a folder
            string[] files = Directory.GetFiles(path);
            string[] folders = Directory.GetDirectories(path);
            // Console.WriteLine("Generating normal page");
            string page = e.html(text:
                              e.head(text: (
                                      e.title(text: "Browsing " + path) +
                                      // e.link(props: new Dictionary<string, string>()
                                      //     {{"rel", "stylesheet"}, {"href", "main.css"}}) +
                                      e.link(props: new Dictionary<string, string>()
                                      {
                                          {"rel", "stylesheet"},
                                          {
                                              "href",
                                              "https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"
                                          }
                                      }) //+
                                      // e.script(props: new Dictionary<string, string>() {{"src", "main.js"}})
                              )) +
                          e.body(text: (
                              e.h1(text: "Browsing " + path) +
                              e.ul(text: (
                                  string.Join("\n", folders.Select(folder => e.li(text: e.a(
                                          props: new Dictionary<string, string>()
                                              {{"href", Path.Join(path, folder.Substring(folder.LastIndexOf('/') + 1))}}, text: "" +
                                          folder.Substring(folder.LastIndexOf('/') + 1)) ,
                                      classes: new string[] {"folder-item"}).ToString())) +
                                  string.Join("\n", files.Select(file => e.li(text: e.a(
                                          props: new Dictionary<string, string>()
                                              {{"href", Path.Join(path, file.Substring(file.LastIndexOf('/') + 1))}}, text: file.Substring(file.LastIndexOf('/') + 1))
                                      ,
                                      classes: new string[] {"file-item"}).ToString()))
                              ))
                          ))
                );
            context.SetContentType("text/html");
            context.TryWriteString(page);
            // Console.WriteLine("Wrote normal page");
            if (context.Open)
                context.Close();
        }
        
    }
}
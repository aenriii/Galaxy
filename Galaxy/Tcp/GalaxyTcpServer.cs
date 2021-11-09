using System;
using System.Net;
using Galaxy;
// using System. //sometihng or other
using Galaxy.Tcp.Internal;
using System.Threading;
using System.Net.Sockets;
using System.Reflection;
using Galaxy.Tcp.Enums;
using Galaxy.Tcp.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Galaxy.Tcp
{
    public class GalaxyTcpServer: IGalaxyTcpServer
    {
        public TcpListener tcpServer;
        public bool isRunning;
        public IGalaxyTcpOptions options;
        public bool utilizeCustomContext;
        public Type? CustomContext;
        public Logger Logger;
        public GalaxyTcpServer( IGalaxyTcpOptions options)
        {
            Logger = new(options.MinimumLogLevel ?? LogLevel.Information);
            this.options = options;
            this.options.Ip ??= IPAddress.Any;
            if (this.options.Port is null)
                throw new ArgumentException("Port must be defined.");
            this.options.EndianOrientation ??= EndianSetting.BigEndian;
            this.options.MaxBufferSize ??= 4096;
            this.options.MaxMessageSize ??= 4096;
            if (options.CustomContext is not null && options.CustomContext.IsSubclassOf(typeof(GalaxyTcpClient)))
            {
                utilizeCustomContext = true;
                CustomContext = options.CustomContext;
                
            }
            else
            {
                utilizeCustomContext = false;
            }
            

        }

        public void Start()
        {
            InitializeTcpServer();
            isRunning = true;
            // if utilizeCustomContext is true and the CustomContext has the bool property PassThrough set to false,
            // the handling will be done by the custom context meaning that we'll have to handle accepting the messages
            // This should only require a call to AcceptTcpClient because if the bool property PassThrough is set to false,
            // it will simply return null. If it is set to true, it is handled by the end-user calling the AcceptTcpClient method
            // themselves, where we won't need to do it.
            if (utilizeCustomContext && !(CustomContext?.GetProperty("PassThrough")?.GetValue(null) is bool passThrough))
            {
                while (AcceptTcpClient() is null)
                    Logger.Info("Custom Client handled a new connection!");
            }
            
        }

        

        public void Stop()
        {
            throw new NotImplementedException();
        }

        private void InitializeTcpServer()
        {
            tcpServer = new TcpListener(this.options.Ip ?? throw new InvalidOperationException(),
                this.options.Port.Value ); // for now
            tcpServer.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            tcpServer.Server.NoDelay = true;
            tcpServer.Server.LingerState = new LingerOption(true, 10);
            tcpServer.Start();
        }
        public object AcceptTcpClient() // Object will inherit or be GalaxyTcpClient, but cant pull that off rn.
        {
            Logger.Info("Accepting Tcp Client...");
            if (tcpServer is null)
                throw new InvalidOperationException("TcpServer is not initialized.");
            if (!isRunning)
                throw new InvalidOperationException("TcpServer is not running.");
            var client = tcpServer.AcceptTcpClient();
            if (utilizeCustomContext)
            {
                Assert.IsTrue(CustomContext is not null, "CustomContext IS null when it should not be"); 
                Assert.IsTrue(CustomContext.IsSubclassOf(typeof(GalaxyTcpClient)), "CustomContext is not a subclass of GalaxyTcpClient when it should be.");
                var clientContext = CustomContext.GetConstructors()[0].Invoke(new object?[] {});
                Logger.Info("Initialized Custom Context");
                if ((bool?) (CustomContext?.GetProperty("PassThrough", BindingFlags.Static)?.GetValue(null)) ??
                    false) // single stupidest way to do this, as far as im concerned.
                    return clientContext;
                // start a thread calling the client's handle method
                var thread = new Thread(() =>
                {
                    try
                    {
                        Logger.Info("Thread enters, handling Tcp request.");
                        // get the handle method
                        var handleMethod = CustomContext.GetMethod("Handle");
                        // call the handle method
                        handleMethod.Invoke(clientContext, new object?[] { this, client});
                        
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.ToString());
                    }
                });
                thread.Start();
                return null;
            }
            else
            {
                return Convert.ChangeType(client, typeof(GalaxyTcpClient)); // TODO: TinyMapper compat 
            }
        }


        
    }
}
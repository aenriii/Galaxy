using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Galaxy.Tcp;
using Galaxy.Tcp.Enums;
using Galaxy.Tcp.Util;
using Microsoft.Extensions.Logging;

namespace GalaxyHelloWorldTcpServer
{
    class Program
    {
        static GalaxyTcpServer tcpServer = new(new()
        {
            Port = 5959,
            Ip = IPAddress.Any,
            CustomContext = typeof(TcpHandler),
            EndianOrientation = EndianSetting.LittleEndian,
            MaxBufferSize = 4096,
            MaxMessageSize = 4096,
            MinimumLogLevel = LogLevel.Debug

        });

        public static void Main(string[] args)
        {
            tcpServer.Start();
            Console.ReadLine();

        }


    }

    public class TcpHandler : GalaxyTcpClient
    {
        private NetworkStream Stream;
        public GalaxyTcpServer server;
        private int State;
        private Logger logger;
        public static bool PassThrough = false;
        public TcpHandler() {}
        
        public void Handle(GalaxyTcpServer server, TcpClient client)
        {
            Client = client;
            this.logger = server.Logger;
            this.server = server;
            logger.Info("TCP open");

            this.PushHelloWorldMessage();
        }
        public void PushHelloWorldMessage()
        {
            byte[] message = Encoding.UTF32.GetBytes("Hello, World!");
            Stream = this.Client.GetStream();
            Stream.Write(message);
            Stream.Flush();
            logger.Info("Sent Hello World, closing");
            Client.Close();
            
        }
    }
}

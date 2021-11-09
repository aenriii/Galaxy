using System;
using System.Net;
using Galaxy.Tcp;
using Galaxy.Tcp.Enums;
using Microsoft.Extensions.Logging;

namespace GalaxyExampleServer
{
    class Program
    {
        static GalaxyTcpServer tcpServer = new( new()
        {
            Port = 25565,
            Ip = IPAddress.Any,
            CustomContext = typeof(Minecraft.Core.TcpHandler),
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
}
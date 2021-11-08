using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text;
using Galaxy;
using Galaxy.Http;
using Galaxy.Tcp
namespace GalaxyExampleServer
{
    class Program
    {
        static GalaxyTcpServer tcpServer = new();
        public static void Main(string[] args)
        {
            // Minecraft MOTD!!!
            tcpServer.UseConnectionHandler((IGalaxyTcpConnectionHandler)Minecraft.Core.TcpHandler)
        }

        
    }
}
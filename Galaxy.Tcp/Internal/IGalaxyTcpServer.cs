using System;
using System.Net.Sockets;
namespace Galaxy.Tcp.Internal
{
    interface IGalaxyTcpServer
    {
        // constructor not included //
        void Start();
        void Stop();
        // GalaxyTcpClient AcceptTcpClient();

    }
}
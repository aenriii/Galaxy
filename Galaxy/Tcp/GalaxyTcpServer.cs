using Galaxy;
// using System. //sometihng or other
using Galaxy.Tcp.Internal;
using System.Threading;
using System.Net.Sockets;
namespace Galaxy.Tcp
{
    public class GalaxyTcpServer<TContext, TConnectionHandler, TParser> : IGalaxyTcpServer <TContext, TConnectionHandler, TParser> where TConnectionHandler : Function<TcpClient, TContext> where TParser : Action<TContext>
    {
        private Type Context = TContext;
        private Type ConnectionHandler = TConnectionHandler;
        private Type Parser = TParser;
        private TcpServer tcpServer;
        public GalaxyTcpServer()
        {
            InitializeTcpServer();
            BeginListeningLoop();
        }
        internal void BeginListeningLoop()
        {
            tcpServer.
        }
    }
}
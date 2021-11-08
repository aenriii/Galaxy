using System.Net.Sockets;
namespace Galaxy.Tcp.Internal
{
    interface IGalaxyTcpServer<TContext, TConnectionHandler, TParser> where TParser : Action<TContext> 
                                                                      where TConnectionHandler : Function<TcpClient, TContext>
    {
        // constructor not included //
        public void UseConnectionHandler(TConnectionHandler handler);
        public void UseContextGenerator(TParser parser);
        
        public void OpenServer();
        
    }
}
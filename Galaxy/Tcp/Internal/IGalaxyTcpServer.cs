
namespace Galaxy.Tcp.Internal
{
    interface IGalaxyTcpServer
    {
        // constructor not included //
        public void UseConnectionHandler(IGalaxyTcpConnectionHandler handler);
        public void OpenServer();
        
    }
}
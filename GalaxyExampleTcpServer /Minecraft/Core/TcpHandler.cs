using Galaxy.Tcp.Internal;

namespace Minecraft.Core
{
    // [SupportsProtocol(756, "1.17.1")]
    public class TcpHandler
    {
        IGalaxyTcpServer _server;
        public TcpHandler(IGalaxyTcpServer server)
        {
            this._server = server;
        }
        public void HandleIncomingRequest(IGalaxyTcpRequest req)
        {
            var data = _server.UnloadData(req); // unloads binary data as stream from req

        }


        #region Conversion
        public static implicit operator IGalaxyTcpConnectionHandler(TcpHandler tcp)
        {
            return (IGalaxyTcpConnectionHandler)tcp.HandleIncomingRequest;
        }
        #endregion
    }

    

}
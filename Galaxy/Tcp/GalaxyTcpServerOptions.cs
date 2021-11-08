using Galaxy.Tcp.Enums;


namespace Galaxy.Tcp
{
    public abstract class GalaxyTcpServerOptions
    {
        public TcpIpOption IpOption;
        public TcpType TcpType;
        public IpAddress Ip = IpAddress.Any;


    }
}
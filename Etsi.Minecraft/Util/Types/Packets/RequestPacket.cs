using System.IO;
using Galaxy.Tcp;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class RequestPacket : MinecraftPacket
    {
        public RequestPacket(GalaxyTcpClient stream) : base(stream) {}
        public RequestPacket(PacketMetadata meta) : base(meta) {}
    }
}
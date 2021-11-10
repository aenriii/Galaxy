using System.IO;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class RequestPacket : MinecraftPacket
    {
        public RequestPacket(Stream stream) : base(stream) {}
        public RequestPacket(PacketMetadata meta) : base(meta) {}
    }
}
using System.IO;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class PacketMetadata
    {
        public VarInt Id;
        public VarInt Length;
        public Stream Body;
    }
}
using System.IO;

namespace Minecraft.Types.Packets
{
    public class RequestPacket : IMinecraftPacket
    {
        public RequestPacket(Stream stream)
        {
            
        }

        public PacketType Type { get; } = PacketType.Generic;
    }
}
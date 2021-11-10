using System.IO;
using Etsi.Minecraft.Server;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class PingPacket : MinecraftPacket
    {
        public long PingId;
        public PingPacket(Stream stream) : base(stream)
        {
            NextSteps();
        } 
        public PingPacket(PacketMetadata meta) : base(meta)
        {
            NextSteps();
        } 
        public void NextSteps()
        {
            // Read the ping ID
            PingId = TcpUtility.ReadLong(_packetData);
        }
        
    }
    
}
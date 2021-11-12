using System.IO;
using Etsi.Minecraft.Server;
using Galaxy.Tcp;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class PingPacket : MinecraftPacket
    {
        public long PingId;
        public PingPacket(GalaxyTcpClient stream) : base(stream)
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
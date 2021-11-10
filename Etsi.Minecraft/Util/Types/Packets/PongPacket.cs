using System.IO;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class PongPacket : ClientboundMinecraftPacket
    {
        public long pingId;
        public PongPacket (long pingId)
        {
            this.Id = 0x01;
            this.pingId = pingId;
        }

        public void Send(Stream stream)
        {
            base.Send(stream);
            OutboundWriter.Write(pingId);
            new VarInt().Write(stream, (int)OutboundStream.Length);
            OutboundStream.CopyTo(stream);
            stream.Flush();
            
        }
        
    }
}
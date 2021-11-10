using System.IO;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class ResponsePacket : ClientboundMinecraftPacket
    {
        
        public byte[] Payload { get; set; }
        public ResponsePacket() : base() {}
        public ResponsePacket(byte[] payload) : base()
        {
            Payload = payload;
            Id = 0x00;
        }
        
        public void SetPayload(byte[] payload)
        {
            Payload = payload;
        }
        public void Send(Stream stream)
        {
            base.Send(stream);
            
            // Write the payload
            OutboundWriter.Write(Payload);
            new VarInt().Write(stream, (int)OutboundStream.Length);
            OutboundStream.CopyTo(stream);
            stream.Flush();
        }
        
    }
}
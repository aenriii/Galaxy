using System.IO;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class ClientboundMinecraftPacket : MinecraftPacket
    {
        public MemoryStream OutboundStream;
        public BinaryWriter OutboundWriter;
        
        public ClientboundMinecraftPacket()
        {
            // Assign the stream and writer
            OutboundStream = new MemoryStream();
            OutboundWriter = new BinaryWriter(OutboundStream);
            
        }

        public void Send(Stream stream)
        {

            // Write the packet ID
            OutboundWriter.Write((byte)Id);
            // Delegate the rest to inheriting classes.
            
            
            
        }
        
        

    }
}
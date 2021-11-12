using System;
using System.IO;
using System.Threading;
using Etsi.Minecraft.Server;
using Galaxy.Tcp;

namespace Etsi.Minecraft.Util.Types.Packets
{
    public class MinecraftPacket : IDisposable
    {
        public Stream _packetData;
        public MinecraftPacketType Type;
        public VarInt Length; // can also be used as int.
        public VarInt Id; // can also be used as int.
        public int _offs; 
        /*
         * Minecraft Packet Format
         * VarInt Length
         * VarInt Type
         * Byte[] Data (this will be what's in the packetData stream)
         */
        public MinecraftPacket() {}
        public MinecraftPacket(GalaxyTcpClient OpenStream)
        {
            // Await stream ready
            VarInt Length = TcpUtility.ReadVarInt(OpenStream);
            // Now we have the length, we can read the rest of the packet
            byte[] buffer = TcpUtility.ReadBytes(OpenStream, Length);
            this._packetData =  new MemoryStream(buffer); 
            // _packetData includes the type as a VarInt and the data as binary.
            // We can now read the type.
            this.Id = TcpUtility.ReadVarInt(_packetData);
            // this.Type = (MinecraftPacketType)Id;
            // Now only data is in the stream, we can delegate this off to inheriting classes.
        }

        public MinecraftPacket(PacketMetadata metadata)
        {
            // Map the properties to the packet.
            this.Type = (MinecraftPacketType) ((int)metadata.Id);
            this.Length = metadata.Length;
            this.Id = metadata.Id;
            this._packetData = metadata.Body;
        }

        public void Dispose()
        {
            _packetData?.Dispose();
        }
    }
}
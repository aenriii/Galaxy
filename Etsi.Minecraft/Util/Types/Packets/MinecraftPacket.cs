using System;
using System.IO;
using System.Threading;

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
        public MinecraftPacket(Stream OpenStream)
        {
            // Await stream ready
            OpenStream.ReadTimeout = 1000;
            byte[] buffer = new byte[] { };
            OpenStream.ReadAsync(buffer, 0, 5).Wait();
            VarInt Length = new VarInt();
            Length.Read(buffer, ref _offs);
            // Now we have the length, we can read the rest of the packet
            buffer = new byte[Length];
            OpenStream.ReadAsync(buffer, 0, Length).Wait();
            this._packetData =  new MemoryStream(buffer); 
            // _packetData includes the type as a VarInt and the data as binary.
            // We can now read the type.
            buffer = new byte[] { };
            this._packetData.Read(buffer, 0, 5);
            VarInt Id = new VarInt();
            Id.Read(buffer, ref _offs);
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
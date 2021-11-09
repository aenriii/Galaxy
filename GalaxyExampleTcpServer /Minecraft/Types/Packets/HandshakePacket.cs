using System;
using System.IO;
using System.Runtime.InteropServices;
using Minecraft.Core;

namespace Minecraft.Types.Packets
{
    public class HandshakePacket : IMinecraftPacket
    {
        

        public PacketType Type => PacketType.Generic;
        public char[] ServerAddress = new char[255];
        public ushort ServerPort;
        public int NextState;

        public HandshakePacket(Stream stream, TcpHandler handler)
        {
            // Assume the stream is already at the correct position
            // Read the server address by getting the length of the string, which is encoded as a VarInt,
            // then read the string.
            byte[] serverAddressLengthBytes = new byte[5];
            stream.Read(serverAddressLengthBytes, 0, 5);
            int x = 0;
            var serverAddressLength = VarInt.ReadVarInt(serverAddressLengthBytes, ref x);
            if (serverAddressLength > 255)
            {
                throw new Exception("Server address is too long");
            }
            byte[] serverAddressBytes = new byte[serverAddressLength];
            stream.Read(serverAddressBytes, 0, serverAddressLength);
            ServerAddress = new char[serverAddressLength];
            
            // Read the server port
            

            if (!handler.TryGetFromBuffer<ushort>(out ServerPort))
                handler.server.Logger.Error("ServerPort UShort unavailable.");
            
            
            // Read the next state, which is technically a VarInt
            byte[] nextStateBytes = new byte[5];
            stream.Read(nextStateBytes, 0, 5);
            x = 0;
            NextState = VarInt.ReadVarInt(nextStateBytes, ref x);
            // Handshake packet is initialized, so the server can transition to state NextState.
        }
        
    }
}
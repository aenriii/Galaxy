using System;
using System.IO;
using Etsi.Minecraft.Server;


namespace Etsi.Minecraft.Util.Types.Packets
{
    public class HandshakePacket : MinecraftPacket
    {
        public VarInt ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        public VarInt NextState;
        public HandshakePacket(Stream stream)
            : base(stream)
        {
            // Note: The first packet sent by the client is a HandshakePacket. Always. I hope.
            
            // HandshakePacket is sent by the client when it first connects to the server.
            // It contains the protocol version, the server's hostname, the server's port, and the next state.
            // The server responds by setting the next planned action to the "nextState" property.
            // Assuming it is a state 1 (status), the server will await a RequestPacket.
            // If the next planned action is not a state 1, the server will throw an exception,
            // as logging in is not currently implemented.
            
            // Read the parameters in order.
            
            NextSteps(this._packetData);
        }

        public HandshakePacket(PacketMetadata metadata) : base(metadata)
        {
            NextSteps(this._packetData);
        }

        public void NextSteps(Stream stream)
        {
            ProtocolVersion = TcpUtility.ReadVarInt(stream);
            ServerAddress = TcpUtility.ReadString(stream);
            ServerPort = TcpUtility.ReadUShort(stream);
            NextState = TcpUtility.ReadVarInt(stream);
            if ((int) ProtocolVersion != (int) Protocol.j1_17_1)
            {
                throw new Exception("Unsupported Protocol Version, " + (int) ProtocolVersion);
            }
            // Now, after having asserted that the protocol version is correct,
            // we can check if the next planned action is a state 1 (status).
            // If it is not, we throw an exception.
            if (NextState != 1)
            {
                throw new Exception("Unsupported Next State, " + (int) NextState);
            }
            // If the next planned action is a state 1 (status),
            // the server will await a RequestPacket.
            // If the next planned action is not a state 1, the server will throw an exception,
            // as logging in is not currently implemented.
            
        }
    }
}
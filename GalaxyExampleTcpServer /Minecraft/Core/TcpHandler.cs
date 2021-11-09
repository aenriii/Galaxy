using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Galaxy.Tcp;
using Galaxy.Tcp.Util;
using Minecraft.Core.Objects;
using Minecraft.Types;
using Minecraft.Types.Packets;
using Minecraft.Types.Payloads;
using Newtonsoft.Json;
using UUIDNext;
using Version = Minecraft.Core.Objects.Version;

namespace Minecraft.Core
{
    // [SupportsProtocol(756, "1.17.1")]
    public class TcpHandler : GalaxyTcpClient
    {
        private NetworkStream Stream;
        public GalaxyTcpServer server;
        public TcpHandler() {}


        
        private int State;
        private Logger logger;
        public static bool PassThrough = false;
        

        public override void Handle(GalaxyTcpServer server, TcpClient client)
        {
            Client = client;
            this.logger = server.Logger;
            this.server = server;
            logger.Info("TCP open");
            // At this point, assert that this has been given a client.
            
            Assert.IsTrue(this.Client.Connected, "TCP Relay claims disconnected on handle. Failing out of context.");
            
            // Read the packet.
            IMinecraftPacket packet = this.ReadPacket(expectHandshake:true);
            if (packet is HandshakePacket)
            {
                logger.Info("Recieved HANDSHAKE.");
                var _packet = packet as HandshakePacket;
                switch (_packet.NextState)
                {
                    case 1:
                        // Next state is 'Status'
                        this.State = 1;
                        logger.Info("Transitioning to State:STATUS");
                        PrepareStatusPacket();
                        break;
                    case 2:
                        // Next state is 'Login'
                        this.State = 2;
                        logger.Info("Transitioning to State:LOGIN // NOT IMPLEMENTED, CLOSING.");
                        Client.Close();
                        throw new System.NotImplementedException("Client tried to login!");
                        break;
                }
            }
            
            
            
        }

        internal void PrepareStatusPacket()
        {
            logger.Info("Making StatusPayload...");
            StatusPayload payload = new StatusPayload(
                default(Version),
                new StatusPayload._Players(420, 69, new Player[] {new Player("jisj", Uuid.NewRandom().ToString())}),
                new ChatMessage("C# Minecraft Server running on the Galaxy TCP library!"),
                Constants.TempFavicon
            );
            logger.Info("StatusPayload created.");
            logger.Info("Reading Request(?) Packet");
            IMinecraftPacket packet = ReadPacket();
            if (packet is RequestPacket)
            {
                logger.Info("RequestPacket confirmed, all go for RESPONSEPACKET with StatusPayload");
                // use statuspayload for JSON payload response for minecraft packet
                this.WritePacket(new ResponsePacket(payload));
            }

        }
        internal IMinecraftPacket ReadPacket(bool expectHandshake = false)
        {
            // get 5 bytes from the Stream
            byte[] packetTypeByte = new byte[5];
            if (Stream is null)
                Stream = Client.GetStream();
            logger.Info("Reading packet type byte");
            WaitUntil(() => Stream.DataAvailable, "Stream is not available.");
            logger.Info("Done Waiting for DataAvailable, reading from packet");
            Stream.Read(packetTypeByte, 0, 5);
            // get the packet id
            int x = 0;
            int packetId = VarInt.ReadVarInt(packetTypeByte, ref x);
            logger.Info("Processing packet with ID" + packetId);
            switch (packetId)
            {
                
                case (int)PacketType.Generic:
                    if (expectHandshake)
                    {
                        // Read the handshake packet.
                        return new HandshakePacket(Stream, this);
                    }
                    else
                    {
                        // Read the generic packet.
                        return new RequestPacket(Stream);
                        
                    }

                    break;
                
            }
            return default(IMinecraftPacket);
        }

        private void WaitUntil(Func<bool> func, string streamIsNotAvailable)
        {
            logger.Info("Waiting...");
            while (!func())
            {
                Thread.Sleep(1);
            }
        }

        internal void WritePacket(IMinecraftPacket packet)
        {
            if (packet is ResponsePacket)
            {
                var _packet = (packet as ResponsePacket);
                byte[] packetTypeByte;
                VarInt.WriteVarInt((int)PacketType.Generic, out packetTypeByte);
                byte[] payload = null;
                if (_packet.Payload is StatusPayload)
                {
                    // turn JSON into byte[]
                    var json = JsonConvert.SerializeObject(_packet.Payload);
                    payload = Encoding.UTF8.GetBytes(json);
                }

                byte[] packetLength;
                VarInt.WriteVarInt(payload.Length, out packetLength);
                byte[] bytePacket = packetTypeByte.Concat(packetLength).Concat(payload).ToArray();
                logger.Info("PACKET CONCATENATED, WRITING AND FLUSHING!!!!");
                Stream.Write(bytePacket, 0, bytePacket.Length);
                Stream.Flush();
            }
        }
    }

    

}
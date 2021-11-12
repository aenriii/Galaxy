using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Etsi.Minecraft;
using Etsi.Minecraft.Server;
using Etsi.Minecraft.Util;
using Etsi.Minecraft.Util.Types.Packets;
using Galaxy.Tcp;
using Newtonsoft.Json;

namespace GalaxyExampleTcpServer
{
    public class TcpHandler : GalaxyTcpClient
    {
        internal GalaxyTcpClient InternalClient;

        public TcpHandler() : base()
        {
        }
        // Run through a standard Minecraft protocol handshake and status request
        public new void Handle(GalaxyTcpServer server, TcpClient client)
        {
            client.ReceiveBufferSize = 4096;
            Client = client;
            #if DEBUG
            Console.WriteLine("Handling client");
            #endif
            Wait.For(() => Assert.StreamHasData(client.GetStream()), waitBefore:true);
            // Since it's the first packet, read Handshake packet
            this.Stream = client.GetStream();
            #if DEBUG
            Console.WriteLine("Grabbing HandshakePacket...");
            #endif
            HandshakePacket handshake = new HandshakePacket(ToGalaxyTcpClient(this));
            // No other processing is neccessary, but let's log some debug info
            #if DEBUG
            Console.WriteLine("Handshake: {0}:{1}", handshake.ServerAddress, handshake.ServerPort);
            Console.WriteLine("Id: " + handshake.Id);
            #endif
            // Store the NextState int and then free up the memory held by the handshake packet
            int nextState = handshake.NextState;
            handshake.Dispose();
            Assert.IsTrue(nextState == 1, "Wrong state, client is trying to log in.");
            #if DEBUG
            Console.WriteLine("State is asserted.");
            #endif
            // Now we can read the next packet, which is a RequestPacket (due to process of events)
            RequestPacket request = new RequestPacket(ToGalaxyTcpClient(this));
            #if DEBUG
            Console.WriteLine("Processed RequestPacket");
            #endif
            // There's no data coming from the RequestPacket, so we can just free up the memory
            request.Dispose();
            // Post the ResponsePacket with the default StatusPayload
            ResponsePacket response = new ResponsePacket(
                Encoding.UTF32.GetBytes(
                    JsonConvert.SerializeObject(Defaults.DefaultStatusPayload)
                )
            );
#if DEBUG
            Console.WriteLine("Generated ResponsePacket");
#endif
            // Send the response
            response.Send(Stream);
            // Free up the memory held by the response packet
            response.Dispose();
#if DEBUG
            Console.WriteLine("Sent and disposed ResponsePacket containing payload of StatusPayload");
#endif
            // Now we can read the next packet, which is a PingPacket (due to process of events)
            PingPacket ping = new PingPacket(Stream);
            // save the pingId for the PongPacket
#if DEBUG
            Console.WriteLine("Read PingPacket; grabbing id for later and disposing.");
#endif
            long pingId = ping.PingId;
            // Free up the memory held by the ping packet
            ping.Dispose();
#if DEBUG
            Console.WriteLine("Making PongPacket");
#endif
            PongPacket pong = new PongPacket(pingId);
            // Send the pong packet
#if DEBUG
            Console.WriteLine("Sending PongPacket");
#endif
            pong.Send(Stream);
            // Free up the memory held by the pong packet
#if DEBUG
            Console.WriteLine("PongPacket sent, disposing.");
#endif
            pong.Dispose();
            // Close the connection, more later.
#if DEBUG
            Console.WriteLine("Client handled, disconnecting.");
#endif
            client.Close();
            
            
            
        }

        public static GalaxyTcpClient ToGalaxyTcpClient(TcpHandler handler)
        {
            return new GalaxyTcpClient()
            {
                Client = handler.Client,
                Stream = handler.Stream,
                galaxyOptions = handler.galaxyOptions
            };
        }
    }
}
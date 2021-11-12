using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using Galaxy.Tcp.Enums;
using Galaxy.Tcp.Internal;
using Galaxy.Tcp.Util;

namespace Galaxy.Tcp
{
    public class GalaxyTcpClient 
    {
        public IGalaxyTcpOptions galaxyOptions;
        public Stream Stream;
        public TcpClient Client;
        public static bool PassThrough = true; // This defines if the context should be passed onto
                                               // "AcceptTcpClient" or if it can handle the request upon class creation
        
        public GalaxyTcpClient()
        {
        }
        public void Use(TcpClient client)
        {
            this.Client = client;
            this.Stream = client.GetStream();
        }



        

        public GalaxyTcpClient SetGalaxyOptions(IGalaxyTcpOptions opts)
        {
            this.galaxyOptions = opts;
            return this;
        }
        
        #region Write buffer to stream

        public bool TryWriteToStream(byte[] bfr, int offset, int count, bool flush = false)
        {
            if (!Stream.CanWrite)
                Wait.For(() => Stream.CanWrite);
            bool isWritten = WaitHandle.WaitAll(
                new[]
                {
                    this.Stream.BeginWrite(
                        bfr,
                        offset,
                        count,
                        (x) => { this.Stream.EndWrite(x); },
                        null
                    ).AsyncWaitHandle
                }
            );
            if (flush && isWritten)
            {
                this.Stream.Flush();
            }
            return isWritten;
                
            
        }
        
        #endregion
        
        #region Read buffer from stream

        public bool TryReadFromStream(byte[] bfr, int offset, int count)
        {
            if (Assert.Exists(Client))
            {
                #if DEBUG
                Console.WriteLine("Waiting for Client.Available to be lte count");
                #endif
                Wait.For(() => Client.Available <= count);
                #if DEBUG
                Console.WriteLine("Client.Available is lte count");
                #endif
            }
            else
            {
                #if DEBUG
                Console.WriteLine("Client is null, not waiting for Client.Available as this is likely made by inference on a non-network stream.");
                #endif
            }
            Wait.For(() => Stream.CanRead);
            return WaitHandle.WaitAll(
                new[]
                {
                    this.Stream.BeginRead(
                        bfr,
                        offset,
                        count,
                        (x) => { this.Stream.EndRead(x); },
                        null
                    ).AsyncWaitHandle
                }
            );
                
            
        }
        #endregion

        public void Handle(GalaxyTcpServer server, TcpClient client)
        {
            
        }

        public static implicit operator GalaxyTcpClient(Stream stream)
        {
            return new GalaxyTcpClient()
            {
                Stream = stream 
            };
        }
        


    }
}
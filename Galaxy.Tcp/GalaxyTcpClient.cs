using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Galaxy.Tcp.Enums;
using Galaxy.Tcp.Internal;

namespace Galaxy.Tcp
{
    public class GalaxyTcpClient 
    {
        public IGalaxyTcpOptions galaxyOptions;
        public NetworkStream Stream;
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

        public bool TryWriteBuffer(byte[] bfr, bool flush = false)
        {
            if (bfr == null)
                throw new ArgumentNullException(nameof(bfr));
            
            if (bfr.Length == 0)
                throw new ArgumentException("Buffer is empty", nameof(bfr));
            // make sure to pay attention to endian-ness
            if (galaxyOptions.EndianOrientation == EndianSetting.LittleEndian)
                Array.Reverse(bfr);
            this.Stream.Write(bfr);
            if (flush)
                this.Stream.Flush();
            return true;
            
            
        }
        
        #endregion
        
        #region Read buffer from stream
        public bool TryGetFromBuffer<T>(out T? val)
        {
            var bfr = new byte[Marshal.SizeOf(typeof(T))];
            this.Stream.Read(bfr, 0, Marshal.SizeOf(typeof(T)));
            if (galaxyOptions.EndianOrientation == EndianSetting.LittleEndian)
                Array.Reverse(bfr);
            // use Marshal to convert the byte array to the type T, return null upon error or failure
            try
            {
                val = (T?) Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(bfr, 0), typeof(T));
                return true;
            }
            catch (Exception)
            {
                val = default(T);
                return false;
            }
        }
        #endregion

        public virtual void Handle(GalaxyTcpServer server, TcpClient client)
        {
            
        }


    }
}
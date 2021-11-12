using System;
using System.IO;
using System.Text;
using Etsi.Minecraft.Util.Types;
using Etsi.Minecraft.Util.Types.Packets;
using Galaxy.Tcp;

namespace Etsi.Minecraft.Server
{
    public class TcpUtility
    {
        public static void WriteString(GalaxyTcpClient client, string str)
        {
            // Strings are written as a length-prefixed string, followed by the string itself.
            // The length is a signed short, in network byte order.
            // The string is UTF-8 encoded.
            
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            byte[] strLengthBytes = BitConverter.GetBytes((short)strBytes.Length);
            client.TryWriteToStream(strLengthBytes, 0, strLengthBytes.Length);
            client.TryWriteToStream(strBytes, 0, strBytes.Length);
        }
        public static string ReadString(GalaxyTcpClient client)
        {
            // Strings are read as a length-prefixed string, followed by the string itself.
            // The length is a signed short, in network byte order.
            // The string is UTF-8 encoded.
            
            byte[] strLengthBytes = new byte[2];
            
            short strLength = BitConverter.ToInt16(strLengthBytes, 0);
            byte[] strBytes = new byte[strLength];
            client.TryReadFromStream(strBytes, 0, strBytes.Length);
            return Encoding.UTF8.GetString(strBytes);
        }
        public static int ReadInt8(GalaxyTcpClient client)
        {
            // Integers are read as a signed byte.
            byte[] intBytes = new byte[1];
            client.TryReadFromStream(intBytes, 0, intBytes.Length);
            return intBytes[0];
        }
        public static void WriteInt8(GalaxyTcpClient client, int int8)
        {
            // Integers are written as a signed byte.
            byte[] intBytes = BitConverter.GetBytes((byte)int8);
            client.TryWriteToStream(intBytes, 0, intBytes.Length);
        }
        public static VarInt ReadVarInt(GalaxyTcpClient client)
        {
            int _offs = 0;
            byte[] buffer = new byte[] { };
            client.TryReadFromStream(buffer, 0, 5);
            VarInt Vint = new VarInt();
            Vint.Read(buffer, ref _offs);
            return Vint;
        }
        public static void WriteVarInt(GalaxyTcpClient client, VarInt varInt)
        {
            int _offs = 0;
            byte[] buffer = new byte[5];
            varInt.Write(buffer, ref _offs);
            client.TryWriteToStream(buffer, 0, buffer.Length);
        }
        public static VarLong ReadVarLong(GalaxyTcpClient client)
        {
            int _offs = 0;
            byte[] buffer = new byte[] { };
            client.TryReadFromStream(buffer, 0, 10);
            VarLong Vlong = new VarLong();
            Vlong.Read(buffer, ref _offs);
            return Vlong;
        }
        public static void WriteVarLong(GalaxyTcpClient client, VarLong varLong)
        {
            int _offs = 0;
            byte[] buffer = new byte[10];
            varLong.Write(buffer, ref _offs);
            client.TryWriteToStream(buffer, 0, buffer.Length);
        }
        public static ushort ReadUShort(GalaxyTcpClient client)
        {
            // Unsigned shorts are read as a signed short, in network byte order.
            byte[] shortBytes = new byte[2];
            client.TryReadFromStream(shortBytes, 0, shortBytes.Length);
            return BitConverter.ToUInt16(shortBytes, 0);
        }
        public static void WriteUShort(GalaxyTcpClient client, ushort sUshort)
        {
            // Unsigned shorts are written as a signed short, in network byte order.
            byte[] shortBytes = BitConverter.GetBytes(sUshort);
            client.TryWriteToStream(shortBytes, 0, shortBytes.Length);
        }
        public static long ReadLong(GalaxyTcpClient client)
        {
            // Signed longs are read as a signed long, in network byte order.
            byte[] longBytes = new byte[8];
            client.TryReadFromStream(longBytes, 0, longBytes.Length);
            return BitConverter.ToInt64(longBytes, 0);
        }
        public static void WriteLong(GalaxyTcpClient client, long long64)
        {
            // Signed longs are written as a signed long, in network byte order.
            byte[] longBytes = BitConverter.GetBytes(long64);
            client.TryWriteToStream(longBytes, 0, longBytes.Length);
        }
        public static byte[] ReadBytes(GalaxyTcpClient client, int length)
        {
            byte[] bytes = new byte[length];
            client.TryReadFromStream(bytes, 0, bytes.Length);
            return bytes;
        }
        public static void WriteBytes(GalaxyTcpClient client, byte[] bytes)
        {
            client.TryWriteToStream(bytes, 0, bytes.Length);
        }
            
        public static PacketMetadata ReadPacketMetadata(GalaxyTcpClient client)
        {
            // Await stream ready
            VarInt Length = TcpUtility.ReadVarInt(client);
            // Now we have the length, we can read the rest of the packet
            byte[] buffer = TcpUtility.ReadBytes(client, Length);
            Stream _packetData =  new MemoryStream(buffer); 
            // _packetData includes the type as a VarInt and the data as binary.
            // We can now read the type.
            VarInt Id = TcpUtility.ReadVarInt(_packetData);
            // this.Type = (MinecraftPacketType)Id;
            // Now only data is in the stream, we can delegate this off to inheriting classes.
            return new PacketMetadata()
            {
                Body = _packetData,
                Length = Length,
                Id = Id
            };
        }
    }
}
using System;
using System.IO;
using System.Text;
using Etsi.Minecraft.Util.Types;
using Etsi.Minecraft.Util.Types.Packets;

namespace Etsi.Minecraft.Server
{
    public class TcpUtility
    {
        public static void WriteString(Stream minecraftTcpStream, string str)
        {
            // Strings are written as a length-prefixed string, followed by the string itself.
            // The length is a signed short, in network byte order.
            // The string is UTF-8 encoded.
            
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            byte[] strLengthBytes = BitConverter.GetBytes((short)strBytes.Length);
            minecraftTcpStream.Write(strLengthBytes, 0, strLengthBytes.Length);
            minecraftTcpStream.Write(strBytes, 0, strBytes.Length);
        }
        public static string ReadString(Stream minecraftTcpStream)
        {
            // Strings are read as a length-prefixed string, followed by the string itself.
            // The length is a signed short, in network byte order.
            // The string is UTF-8 encoded.
            
            byte[] strLengthBytes = new byte[2];
            minecraftTcpStream.ReadAsync(strLengthBytes, 0, strLengthBytes.Length).Wait();
            short strLength = BitConverter.ToInt16(strLengthBytes, 0);
            byte[] strBytes = new byte[strLength];
            minecraftTcpStream.ReadAsync(strBytes, 0, strBytes.Length).Wait();
            return Encoding.UTF8.GetString(strBytes);
        }
        public static int ReadInt8(Stream minecraftTcpStream)
        {
            // Integers are read as a signed byte.
            byte[] intBytes = new byte[1];
            minecraftTcpStream.ReadAsync(intBytes, 0, intBytes.Length).Wait();
            return intBytes[0];
        }
        public static void WriteInt8(Stream minecraftTcpStream, int int8)
        {
            // Integers are written as a signed byte.
            byte[] intBytes = BitConverter.GetBytes((byte)int8);
            minecraftTcpStream.Write(intBytes, 0, intBytes.Length);
        }
        public static VarInt ReadVarInt(Stream minecraftTcpStream)
        {
            int _offs = 0;
            byte[] buffer = new byte[] { };
            minecraftTcpStream.ReadAsync(buffer, 0, 5).Wait();
            VarInt Vint = new VarInt();
            Vint.Read(buffer, ref _offs);
            return Vint;
        }
        public static void WriteVarInt(Stream minecraftTcpStream, VarInt varInt)
        {
            int _offs = 0;
            byte[] buffer = new byte[5];
            varInt.Write(buffer, ref _offs);
            minecraftTcpStream.Write(buffer, 0, buffer.Length);
        }
        public static VarLong ReadVarLong(Stream minecraftTcpStream)
        {
            int _offs = 0;
            byte[] buffer = new byte[] { };
            minecraftTcpStream.ReadAsync(buffer, 0, 10).Wait();
            VarLong Vlong = new VarLong();
            Vlong.Read(buffer, ref _offs);
            return Vlong;
        }
        public static void WriteVarLong(Stream minecraftTcpStream, VarLong varLong)
        {
            int _offs = 0;
            byte[] buffer = new byte[10];
            varLong.Write(buffer, ref _offs);
            minecraftTcpStream.Write(buffer, 0, buffer.Length);
        }
        public static ushort ReadUShort(Stream minecraftTcpStream)
        {
            // Unsigned shorts are read as a signed short, in network byte order.
            byte[] shortBytes = new byte[2];
            minecraftTcpStream.ReadAsync(shortBytes, 0, shortBytes.Length).Wait();
            return BitConverter.ToUInt16(shortBytes, 0);
        }
        public static void WriteUShort(Stream minecraftTcpStream, ushort sUshort)
        {
            // Unsigned shorts are written as a signed short, in network byte order.
            byte[] shortBytes = BitConverter.GetBytes(sUshort);
            minecraftTcpStream.Write(shortBytes, 0, shortBytes.Length);
        }
        public static long ReadLong(Stream minecraftTcpStream)
        {
            // Signed longs are read as a signed long, in network byte order.
            byte[] longBytes = new byte[8];
            minecraftTcpStream.ReadAsync(longBytes, 0, longBytes.Length).Wait();
            return BitConverter.ToInt64(longBytes, 0);
        }
        public static void WriteLong(Stream minecraftTcpStream, long long64)
        {
            // Signed longs are written as a signed long, in network byte order.
            byte[] longBytes = BitConverter.GetBytes(long64);
            minecraftTcpStream.Write(longBytes, 0, longBytes.Length);
        }
        
        public static PacketMetadata ReadPacketMetadata(Stream minecraftTcpStream)
        {
            PacketMetadata packetMetadata = new PacketMetadata();
            packetMetadata.Length = ReadVarInt(minecraftTcpStream);
            packetMetadata.Id = ReadVarInt(minecraftTcpStream);
            // Read the remainder of the packet and assign it as a MemoryStream to the Body property.
            byte[] packetBody = new byte[packetMetadata.Length - 1];
            minecraftTcpStream.ReadAsync(packetBody, 0, packetBody.Length).Wait();
            packetMetadata.Body = new MemoryStream(packetBody);
            return packetMetadata;
        }
    }
}
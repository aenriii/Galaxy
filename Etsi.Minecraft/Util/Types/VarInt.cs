using System;
using System.IO;

namespace Etsi.Minecraft.Util.Types
{
    public class VarInt
    {
        int _value;
        public VarInt()
        {}
        public void Read(byte[] data, ref int offset)
        {
            if (data.Length < offset + 1)
                throw new ArgumentException("Not enough data to read VarInt");
            
            
            int value = 0;
            int shift = 0;
            byte b;
            do
            {
                b = data[offset++];
                value |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            offset = 0;
            _value = value;
        }
        public void Write(byte[] data, ref int offset)
        {
            int value = _value;
            do
            {
                data[offset++] = (byte)(value & 0x7F);
                value >>= 7;
            } while (value != 0);
        }
        public void Write(Stream stream, int value)
        {
            _value = value;
            // allocate 5 bytes to write the value to then write the bytes to the stream
            
            byte[] buffer = new byte[5];
            int offset = 0;
            do
            {
                buffer[offset++] = (byte)(value & 0x7F);
                value >>= 7;
            } while (value != 0);
            stream.Write(buffer, 0, offset);
        }
        public static implicit operator int(VarInt v)
        {
            return v._value;
        }
        public static implicit operator VarInt(int v)
        {
            return new VarInt { _value = v };
        }
    }
}
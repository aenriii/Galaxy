using System;

namespace Etsi.Minecraft.Util.Types
{
    public class VarLong
    {
        long _value;
        public VarLong()
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
            long value = _value;
            do
            {
                data[offset++] = (byte)(value & 0x7F);
                value >>= 7;
            } while (value != 0);
        }
        public static implicit operator long(VarLong v)
        {
            return v._value;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
namespace Minecraft.Types
{
    public class VarLong
    {
        // read and write VarLongs
        public static long ReadVarLong(byte[] bytes, ref int offset)
        {
            long result = 0;
            int shift = 0;
            while (true)
            {
                byte b = bytes[offset++];
                result |= (long)(b & 0x7F) << shift;
                if ((b & 0x80) != 0x80)
                    break;
                shift += 7;
            }
            return result;
        }
        public static void WriteVarLong(long value, byte[] bytes, ref int offset)
        {
            while (true)
            {
                byte b = (byte)(value & 0x7F);
                value >>= 7;
                if (value != 0)
                    b |= 0x80;
                bytes[offset++] = b;
                if (value == 0)
                    break;
            }
        }
    }
}

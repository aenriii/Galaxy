using System;
using System.Collections.Generic;
using System.Linq;
namespace Minecraft.Types
{
    public class VarInt
    {
        public static int ReadVarInt(byte[] bytes, ref int offset)
        {
            int result = 0;
            int i = 0;
            while (true)
            {
                int num = bytes[offset++];
                result |= (num & 127) << i;
                if ((num & 128) == 0)
                {
                    return result;
                }
                i += 7;
            }
        }

        public static void WriteVarInt(int value, out byte[] bytes)
        {
            bytes = new byte[] {};
            var offset = 0;
            while ((value & -128) != 0)
            {
                bytes[offset++] = (byte)(value & 127 | 128);
                value = (int)((uint)value >> 7);
            }
            bytes[offset++] = (byte)value;
        }
    }
}
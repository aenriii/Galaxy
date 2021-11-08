using System;
using System.Collections.Generic;
using System.Linq;
namespace Minecraft.Types
{
    public abstract class VarLong
    {
        internal byte[10] value;
        
        public static implicit operator VarLong(byte[] byteArr)
        {
            long value = 0;
            int bitOffset = 0;
            IEnumerator<byte> byteEnum = byteArr.GetEnumerator();
            byte currentByte;
            do {
                if (bitOffset == 70) throw new InvalidCastException("Cannot cast from byte[>10] to VarLong");

                currentByte = byteEnum.Current;
                value |= (currentByte & 0b01111111) << bitOffset;

                bitOffset += 7;
                byteEnum.MoveNext();
            } while ((currentByte & 0b10000000) != 0);

            return value;
        }
        public static implicit operator byte[](VarLong vlong)
        {
            byte[] byteArr = new();
            while (true) {
            // Only the first 7 bits have data
                if ((value & 0b1111111111111111111111111111111111111111111111111111111110000000) == 0) {
                    byteArr.SetValue(value, byteArr.Count());
                    return byteArr;
                }

                byteArr.SetValue(value & 0b01111111 | 0b10000000, byteArr.Count());
                // Note: >>> means that the sign bit is shifted with the rest of the number rather than being left alone
                value >> 7;
            }
            return byteArr;
        }
    }
}

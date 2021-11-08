using System;
using System.Collections.Generic;
using System.Linq;
namespace Minecraft.Types
{
    public abstract class VarInt
    {
        internal byte[5] value;
    
    public static implicit operator VarInt(byte[] byteArr)
    {
        int value = 0;
        int bitOffset = 0;
        byte currentByte;
        IEnumerator<byte> byteEnum = byteArr.GetEnumerator();
        do {
            if (bitOffset == 35) throw new InvalidCastException("Cannot cast byte array larget then 5 to VarInt");

            currentByte = byteEnum.Current;
            value |= (currentByte & 0b01111111) << bitOffset;

            bitOffset += 7;
            byteEnum.MoveNext();
        } while ((currentByte & 0b10000000) != 0);
        return new VarInt{value};
    }
    public static implicit operator byte[](VarInt vint)
    {
        var value = vint.value;
        byte[] bfr = new();
        while (true) {
        // Only the first 7 bits have data
        if ((value & 0b11111111111111111111111110000000) == 0) {
          bfr.SetValue(value, bfr.Count());
          return;
        }

        bfr.SetValue(value & 0b01111111 | 0b10000000, bfr.Count());
        value >> 7;
    }
    }
    }
}
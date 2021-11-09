namespace Minecraft.Types
{
    public enum PacketType
    {
        LegacyPing = 0xFE, // Shouldn't need to handle this under the right circumstances.
        Generic = 0x00,
        PingPong = 0x01,
        
    }
}
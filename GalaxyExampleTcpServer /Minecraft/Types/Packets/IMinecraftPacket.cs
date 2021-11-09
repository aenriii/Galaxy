namespace Minecraft.Types.Packets
{
    public interface IMinecraftPacket
    {
        PacketType Type { get; }
        
    }
}
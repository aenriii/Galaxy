namespace Minecraft.Types.Packets
{
    public class ResponsePacket : IMinecraftPacket

    {
    public object? Payload;

    public ResponsePacket(object? payload)
    {
        this.Payload = payload;
    }

    public PacketType Type { get; } = PacketType.Generic;
    }
}
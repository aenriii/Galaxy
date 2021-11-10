namespace Etsi.Minecraft.Util.Types
{
    public enum MinecraftPacketType // 00 and 01 are all we need to worry about right now, leave logins unhandled for now.
    {
        Generic = 0x00, // Request, Response, and Handshake.
        PingPong = 0x01, 
    }
}
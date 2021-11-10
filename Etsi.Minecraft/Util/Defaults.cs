using Etsi.Minecraft.Util.Types;
using Etsi.Minecraft.Util.Types.Payloads;
using UUIDNext;

namespace Etsi.Minecraft.Util
{
    public class Defaults
    {
        public static readonly StatusPayload DefaultStatusPayload = new StatusPayload(
                default(Version),
                new StatusPayload._Players(420, 69, new Player[] {new Player("jisj", Uuid.NewRandom().ToString())}),
                new ChatMessage("C# Minecraft Server running on the Galaxy TCP library!"),
                Constants.TempFavicon
            );
    }
}
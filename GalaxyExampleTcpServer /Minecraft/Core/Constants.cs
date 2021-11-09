using System;

namespace Minecraft.Core
{
    public class Constants
    {
        public static string TempFavicon =
            "data:image/png;base64,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".Replace("\n", String.Empty);
    }
}
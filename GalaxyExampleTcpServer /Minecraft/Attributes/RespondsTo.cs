using System;

namespace Minecraft.Attributes
{
    public class RespondsTo<T> : Attribute where T : IMinecraftEvent
    {
        public RespondsTo(T type)
        {

        }
    }
}
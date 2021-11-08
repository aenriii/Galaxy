using System;

namespace Minecraft.Attributes
{
    public class Event : Attribute
    {
        string Name;
        public Event(string Name)
        {
            this.Name = Name;
        }
    }
}
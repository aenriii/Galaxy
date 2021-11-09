using Newtonsoft.Json;

namespace Minecraft.Core.Objects
{
    public class ChatMessage
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        public ChatMessage(string text)
        {
            Text = text;
        }
    }
}
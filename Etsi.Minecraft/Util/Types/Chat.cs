using Newtonsoft.Json;

namespace Etsi.Minecraft.Util.Types
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
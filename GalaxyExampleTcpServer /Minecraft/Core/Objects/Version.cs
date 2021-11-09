using Newtonsoft.Json;

namespace Minecraft.Core.Objects
{
    public class Version
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "1.17.1";
        [JsonProperty("protocol")]
        public int Protocol { get; set; } = 756;
    }
}
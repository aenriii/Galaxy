using Newtonsoft.Json;

namespace Etsi.Minecraft.Util.Types
{
    public class Version
    {
        [JsonProperty("name")]
        public string Name { get; set; } = "1.17.1";
        [JsonProperty("protocol")]
        public int Protocol { get; set; } = 756;
    }
}
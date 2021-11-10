using Newtonsoft.Json;

namespace Etsi.Minecraft.Util.Types
{
    public class Player
    {
        // constructor
        public Player(string name, string uuid)
        {
            Name = name;
            Id = uuid;
        }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        // more coming soon.
    }
}
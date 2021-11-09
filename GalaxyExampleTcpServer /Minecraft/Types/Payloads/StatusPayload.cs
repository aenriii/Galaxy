using System.Runtime.Serialization;
using Minecraft.Core.Objects;
using Newtonsoft.Json;

namespace Minecraft.Types.Payloads
{
    [JsonObject]
    public class StatusPayload
    {
        public StatusPayload(
            Version version,
            _Players players,
            ChatMessage description,
            string favicon
        )
        {
            Version = version;
            Players = players;
            Description = description;
            Favicon = favicon;
        }

        

        [JsonProperty("version")]
        public Version Version { get; set; }
        [JsonProperty("players")]
        public _Players Players { get; set; }

        public class _Players
        {
            // constructor
            public _Players(
                int max,
                int online,
                Player[] sample
            )
            {
                Max = max;
                Online = online;
                Sample = sample;
            }
            [JsonProperty("max")]
            public int Max { get; set; }
            [JsonProperty("online")]
            public int Online { get; set; }
            [JsonProperty("sample")]
            public Player[] Sample { get; set; }
        }

        [JsonProperty("description")]
        public ChatMessage Description { get; set; }
        [JsonProperty("favicon")]
        public string Favicon { get; set; }

        
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class KeyArtImages
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("h")]
        public int H { get; set; }
        [JsonProperty("w")]
        public int W { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class Alternatives
    {
        [JsonProperty("quality")]
        public string Quality { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

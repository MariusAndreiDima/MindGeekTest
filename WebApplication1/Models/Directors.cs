using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class Directors
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

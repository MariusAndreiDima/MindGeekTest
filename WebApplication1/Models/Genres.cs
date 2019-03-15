using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class Genres
    {
        [JsonProperty("genres")]
        public List<string> Genre { get; set; }
    }
}

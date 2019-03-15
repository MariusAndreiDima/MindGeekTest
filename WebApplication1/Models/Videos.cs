﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class Videos
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("alternatives")]
        public List<Alternatives> Alternatives { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

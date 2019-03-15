using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class ViewingWindow
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        [JsonProperty("wayToWatch")]
        public string WayToWatch { get; set; }
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
    }
}

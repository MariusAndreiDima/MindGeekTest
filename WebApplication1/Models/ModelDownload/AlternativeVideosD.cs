using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class AlternativeVideosD
    {
        public string Movie { get; set; }
        public string Id { get; set; }
        public string Quality { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }

        public AlternativeVideosD(string movie, string id, string quality , string url, string status, string source)
        {
            Movie = movie;
            Id = id;
            Url = url;
            Quality = quality;
            Status = status;
            Source = source;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class VideosD
    {
        public string Movie { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }

        public List<AlternativeVideosD> Alternatives { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public VideosD(string movie, string id, string title , List<AlternativeVideosD> alternatives , string type , string url, string status, string source)
        {
            Movie = movie;
            Id = id;
            Title = title;
            Alternatives = alternatives;
            Type = type;
            Url = url;
            Status = status;
            Source = source;
        }
    }
}

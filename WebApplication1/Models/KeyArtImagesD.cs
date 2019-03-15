using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class KeyArtImagesD
    {
        public string Movie { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public int H { get; set; }
        public int W { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        
        public KeyArtImagesD(string movie, string id, string url, int h, int w, string status, string source)
        {
            Movie = movie;
            Id = id;
            Url = url;
            H = h;
            W = w;
            Status = status;
            Source = source;
        }
    }
}

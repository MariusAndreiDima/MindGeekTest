using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MindGeekTest.Models
{
    public class MoviesDetails
    {
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("cardImages")]
        public List<CardImages> CardImages { get; set; }
        [JsonProperty("cast")]
        public List<Cast> Cast { get; set; }
        [JsonProperty("cert")]
        public string Cert { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("directors")]
        public List<Directors> Directors { get; set; }
        [JsonProperty("duration")]
        public string Duration { get; set; }
        [JsonProperty("genres")]
        public List<string> Genres { get; set; }
        [JsonProperty("headline")]
        public string Headline { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("keyArtImages")]
        public List<KeyArtImages> KeyArtImages { get; set; }
        [JsonProperty("lastUpdated")]
        public string LastUpdated { get; set; }
        [JsonProperty("quote")]
        public string Quote { get; set; }
        [JsonProperty("rating")]
        public int Rating { get; set; }
        [JsonProperty("reviewAuthor")]
        public string ReviewAuthor { get; set; }
        [JsonProperty("skyGoId")]
        public string SkyGoId { get; set; }
        [JsonProperty("SkyGoUrl")]
        public string SkyGoUrl { get; set; }
        [JsonProperty("sum")]
        public string Sum { get; set; }
        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("videos")]
        public List<Videos> Videos { get; set; }
        [JsonProperty("viewingWindow")]
        public ViewingWindow ViewingWindow { get; set; }
        [JsonProperty("year")]
        public string Year { get; set; }
    }
}

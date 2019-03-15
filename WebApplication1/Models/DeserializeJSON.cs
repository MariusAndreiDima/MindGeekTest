using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace MindGeekTest.Models
{
    public class MoviesDetails
    {
        [JsonProperty("body")]
        public string body { get; set; }
        [JsonProperty("cardImages")]
        public List<CardImages> cardImages { get; set; }
        [JsonProperty("cast")]
        public List<Cast> cast { get; set; }
        [JsonProperty("cert")]
        public string cert { get; set; }
        [JsonProperty("class")]
        public string cclass { get; set; }
        [JsonProperty("directors")]
        public List<Directors> directors { get; set; }
        [JsonProperty("duration")]
        public string duration { get; set; }
        [JsonProperty("genres")]
        public List<string> genres { get; set; }
        [JsonProperty("headline")]
        public string headline { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("keyArtImages")]
        public List<KeyArtImages> keyArtImages { get; set; }
        [JsonProperty("lastUpdated")]
        public string lastUpdated { get; set; }
        [JsonProperty("quote")]
        public string quote { get; set; }
        [JsonProperty("rating")]
        public int rating { get; set; }
        [JsonProperty("reviewAuthor")]
        public string reviewAuthor { get; set; }
        [JsonProperty("skyGoId")]
        public string skyGoId { get; set; }
        [JsonProperty("SkyGoUrl")]
        public string skyGoUrl { get; set; }
        [JsonProperty("sum")]
        public string sum { get; set; }
        [JsonProperty("synopsis")]
        public string synopsis { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("videos")]
        public List<Videos> videos { get; set; }
        [JsonProperty("viewingWindow")]
        public ViewingWindow viewingWindow { get; set; }
        [JsonProperty("year")]
        public string year { get; set; }
    }

    public class CardImages
    {
        [JsonProperty("url")]
        [Url]
        public string url { get; set; }
        [JsonProperty("h")]
        public int h { get; set; }
        [JsonProperty("w")]
        public int w { get; set; }

    }

    public class Cast
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class Directors
    {
        [JsonProperty("name")]
        public string name { get; set; }
    }

    public class Genres
    {
        [JsonProperty("genres")]
        public List<string> genre { get; set; }
    }

    public class KeyArtImages
    {
        [JsonProperty("url")]
        public string url { get; set; }
        [JsonProperty("h")]
        public int h { get; set; }
        [JsonProperty("w")]
        public int w { get; set; }
    }
    public class Videos
    {
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("alternatives")]
        public List<Alternatives> alternatives { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }
    public class Alternatives
    {
        [JsonProperty("quality")]
        public string quality { get; set; }
        [JsonProperty("url")]
        public string url { get; set; }
    }
    public class ViewingWindow
    {
        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("startDate")]
        public string startDate { get; set; }
        [JsonProperty("wayToWatch")]
        public string wayToWatch { get; set; }
        [JsonProperty("endDate")]
        public string endDate { get; set; }
    }
}

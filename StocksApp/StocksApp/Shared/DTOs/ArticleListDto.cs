using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StocksApp.Shared.DTOs
{


    public partial class ArticleListDto
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("next_url")]
        public Uri NextUrl { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("published_utc")]
        public DateTimeOffset PublishedUtc { get; set; }

        [JsonProperty("article_url")]
        public Uri ArticleUrl { get; set; }

        [JsonProperty("tickers")]
        public List<string> Tickers { get; set; }

        [JsonProperty("amp_url")]
        public Uri AmpUrl { get; set; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("keywords", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Keywords { get; set; }
    }

    public partial class Publisher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("homepage_url")]
        public Uri HomepageUrl { get; set; }

        [JsonProperty("logo_url")]
        public Uri LogoUrl { get; set; }

        [JsonProperty("favicon_url")]
        public Uri FaviconUrl { get; set; }
    }
}

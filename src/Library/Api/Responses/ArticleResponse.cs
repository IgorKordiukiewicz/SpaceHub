using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record ArticleResponse
    {
        [JsonProperty("id")]
        public int ArticleId { get; init; }
        
        [JsonProperty("title")]
        public string Title { get; init; }

        [JsonProperty("summary")]
        public string Summary { get; init; }

        [JsonProperty("url")]
        public string Url { get; init; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; init; }

        [JsonProperty("newsSite")]
        public string NewsSite { get; init; }

        [JsonProperty("publishedAt")]
        public DateTime PublishDate { get; init; }
    }
}

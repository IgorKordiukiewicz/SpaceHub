using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record ArticleResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }
        
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("summary")]
        public string Summary { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; init; }

        [JsonPropertyName("newsSite")]
        public string NewsSite { get; init; }

        [JsonPropertyName("publishedAt")]
        public DateTime PublishDate { get; init; }

        [JsonPropertyName("updatedAt")]
        public DateTime UpdateDate { get; init; }
    }
}

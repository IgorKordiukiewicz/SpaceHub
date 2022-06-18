using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record VideoUrlResponse
    {
        [JsonPropertyName("priority")]
        public int Prority { get; init; }

        [JsonPropertyName("title")]
        public string? Title { get; init; }

        [JsonPropertyName("description")]
        public string? Description { get; init; }

        [JsonPropertyName("feature_image")]
        public string? ImageUrl { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }
    }
}
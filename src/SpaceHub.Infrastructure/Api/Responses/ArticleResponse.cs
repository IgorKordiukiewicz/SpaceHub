using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

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
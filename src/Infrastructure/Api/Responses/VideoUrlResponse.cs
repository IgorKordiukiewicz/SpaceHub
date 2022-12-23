using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

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

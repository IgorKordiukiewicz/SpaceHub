using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record EventResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("type")]
    public EventTypeResponse Type { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }

    [JsonPropertyName("location")]
    public string Location { get; init; }

    [JsonPropertyName("news_url")]
    public string? NewsUrl { get; init; }

    [JsonPropertyName("video_url")]
    public string? VideoUrl { get; init; }

    [JsonPropertyName("feature_image")]
    public string ImageUrl { get; init; }

    [JsonPropertyName("date")]
    public DateTime? Date { get; init; }

    [JsonPropertyName("launches")]
    public IEnumerable<LaunchResponse> Launches { get; init; }

    [JsonPropertyName("program")]
    public IEnumerable<ProgramResponse> Programs { get; init; }
}

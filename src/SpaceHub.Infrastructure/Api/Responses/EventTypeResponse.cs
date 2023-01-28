using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record EventTypeResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
}

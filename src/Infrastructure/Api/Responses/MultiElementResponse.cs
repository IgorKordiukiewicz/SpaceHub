using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record MultiElementResponse
{
    [JsonPropertyName("count")]
    public int Count { get; init; }
}

using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record RocketsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IEnumerable<RocketConfigResponse> Rockets { get; init; }
}

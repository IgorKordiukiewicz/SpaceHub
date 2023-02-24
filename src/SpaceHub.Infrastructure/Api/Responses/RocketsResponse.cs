using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record RocketsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<RocketConfigResponse> Rockets { get; init; }
}

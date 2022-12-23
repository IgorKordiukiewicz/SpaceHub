using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record RocketsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IEnumerable<RocketConfigResponse> Rockets { get; init; }
}

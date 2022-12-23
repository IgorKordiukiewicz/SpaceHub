using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record RocketsDetailResponse : RocketsResponse
{
    [JsonPropertyName("results")]
    public new IEnumerable<RocketConfigDetailResponse> Rockets { get; init; }
}

using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record RocketsDetailResponse : RocketsResponse
{
    [JsonPropertyName("results")]
    public new IEnumerable<RocketConfigDetailResponse> Rockets { get; init; }
}

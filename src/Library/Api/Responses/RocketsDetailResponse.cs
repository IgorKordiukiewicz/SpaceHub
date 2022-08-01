using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record RocketsDetailResponse : RocketsResponse
{
    [JsonPropertyName("results")]
    public new IEnumerable<RocketConfigDetailResponse> Rockets { get; init; }
}

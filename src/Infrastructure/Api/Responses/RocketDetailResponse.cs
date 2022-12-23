using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record RocketDetailResponse : RocketResponse
{
    [JsonPropertyName("configuration")]
    public new RocketConfigDetailResponse Configuration { get; init; }
}

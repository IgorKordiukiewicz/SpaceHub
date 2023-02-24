using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record LaunchesResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<LaunchResponse> Launches { get; init; }
}

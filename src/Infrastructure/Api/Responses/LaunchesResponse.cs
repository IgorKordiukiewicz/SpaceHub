using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record LaunchesResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IEnumerable<LaunchResponse> Launches { get; init; }
}

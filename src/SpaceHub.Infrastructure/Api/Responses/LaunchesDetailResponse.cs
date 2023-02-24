using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record LaunchesDetailResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<LaunchDetailResponse> Launches { get; set; }
}

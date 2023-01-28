using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record LaunchesDetailResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyCollection<LaunchDetailResponse> Launches { get; set; }
}

using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record AgenciesDetailResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<AgencyDetailResponse> Agencies { get; set; }
}

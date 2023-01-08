using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record AgenciesDetailResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyCollection<AgencyDetailResponse> Agencies { get; set; }
}

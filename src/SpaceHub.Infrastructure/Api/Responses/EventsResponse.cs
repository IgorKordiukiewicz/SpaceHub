using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record EventsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IReadOnlyList<EventResponse> Events { get; init; }
}

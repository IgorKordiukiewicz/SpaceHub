using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record EventsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IEnumerable<EventResponse> Events { get; init; }
}

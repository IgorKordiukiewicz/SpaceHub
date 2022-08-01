using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record EventsResponse : MultiElementResponse
{
    [JsonPropertyName("results")]
    public IEnumerable<EventResponse> Events { get; init; }
}

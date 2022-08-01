using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record MultiElementResponse
{
    [JsonPropertyName("count")]
    public int Count { get; init; }
}

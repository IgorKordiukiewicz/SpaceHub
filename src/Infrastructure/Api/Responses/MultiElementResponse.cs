using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record MultiElementResponse
{
    [JsonPropertyName("count")]
    public int Count { get; init; }
}

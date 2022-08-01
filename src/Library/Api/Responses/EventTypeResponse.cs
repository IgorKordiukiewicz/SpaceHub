using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record EventTypeResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }
}

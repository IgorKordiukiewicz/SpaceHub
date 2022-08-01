using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record ProgramResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }

    [JsonPropertyName("agencies")]
    public IEnumerable<AgencyResponse> Agencies { get; init; }

    [JsonPropertyName("image_url")]
    public string ImageUrl { get; init; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("info_url")]
    public string? InfoUrl { get; init; }

    [JsonPropertyName("wiki_url")]
    public string? WikiUrl { get; init; }
}

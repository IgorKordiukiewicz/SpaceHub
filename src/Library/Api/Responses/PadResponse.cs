using System.Text.Json.Serialization;

namespace Library.Api.Responses;

public record PadResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("url")]
    public string Url { get; init; }

    [JsonPropertyName("agency_id")]
    public int? AgencyId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("info_url")]
    public string? InfoUrl { get; init; }

    [JsonPropertyName("wiki_url")]
    public string? WikiUrl { get; init; }

    [JsonPropertyName("map_url")]
    public string? MapUrl { get; init; }

    [JsonPropertyName("latitude")]
    public string Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public string Longitude { get; init; }

    [JsonPropertyName("location")]
    public PadLocationResponse Location { get; init; }

    [JsonPropertyName("map_image")]
    public string MapImageUrl { get; init; }

    [JsonPropertyName("total_launch_count")]
    public int TotalLaunches { get; init; }
}

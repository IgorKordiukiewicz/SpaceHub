namespace Library.Models;

public record Pad
{
    public int? AgencyId { get; init; }
    public string Name { get; init; }
    public string? InfoUrl { get; init; }
    public string? WikiUrl { get; init; }
    public string? MapUrl { get; init; }
    public string Latitude { get; init; }
    public string Longitude { get; init; }
    public PadLocation Location { get; init; }
    public string MapImageUrl { get; init; }
    public int TotalLaunches { get; init; }
}

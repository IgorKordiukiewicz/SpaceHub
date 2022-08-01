namespace Library.Models;

public record PadLocation
{
    public string Name { get; init; }
    public string CountryCode { get; init; }
    public string MapImageUrl { get; init; }
    public int TotalLaunches { get; init; }
    public int TotalLandings { get; init; }
}

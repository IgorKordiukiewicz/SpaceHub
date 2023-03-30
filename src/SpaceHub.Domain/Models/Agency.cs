namespace SpaceHub.Domain.Models;

public class Agency
{
    public required int ApiId { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string CountryCode { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Administrator { get; set; } = string.Empty;
    public int? FoundingYear { get; init; }
    public int TotalLaunches { get; set; }
    public int SuccessfulLaunches { get; set; }
    public string LogoUrl { get; init; } = string.Empty;
    public string WikiUrl { get; init; } = string.Empty;
    public string InfoUrl { get; init; } = string.Empty;
}
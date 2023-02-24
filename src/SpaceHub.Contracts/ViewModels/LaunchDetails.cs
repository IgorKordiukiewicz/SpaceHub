namespace SpaceHub.Contracts.ViewModels;

public record LaunchDetailsVM
{
    public required AgencyVM Agency { get; init; }
    public required RocketVM Rocket { get; init; }
}

public record AgencyVM
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string ImageUrl { get; init; }
    public required string Type { get; init; }
    public required string CountryCode { get; init; }
    public required string Administrator { get; init; }
    public int? FoundingYear { get; init; }
}

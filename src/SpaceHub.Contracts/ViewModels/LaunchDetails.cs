namespace SpaceHub.Contracts.ViewModels;

public record LaunchDetailsVM
{
    public required AgencyVM Agency { get; init; }
    public required RocketVM Rocket { get; init; }
}

public record AgencyVM
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
}

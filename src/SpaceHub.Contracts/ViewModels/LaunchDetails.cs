namespace SpaceHub.Contracts.ViewModels;

public record LaunchDetailsVM
{
    public required AgencyVM Agency { get; init; }
    public required RocketVM Rocket { get; init; }
}

public record AgencyVM
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
}

public record RocketVM
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string? ImageUrl { get; init; }
    public List<RocketPropertyVM> Properties { get; init; } = new();
}

public record RocketPropertyVM(string Name, string Value, string? Symbol);

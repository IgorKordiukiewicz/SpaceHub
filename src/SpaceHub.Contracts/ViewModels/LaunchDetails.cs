namespace SpaceHub.Contracts.ViewModels;

public record LaunchDetailsVM
{
    public AgencyVM Agency { get; init; }
    public RocketVM Rocket { get; init; }
}

public record AgencyVM
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
}

public record RocketVM
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public List<RocketPropertyVM> Properties { get; init; }
}

public record RocketPropertyVM(string Name, string Value, string? Symbol);

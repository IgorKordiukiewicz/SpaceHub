namespace SpaceHub.Contracts.ViewModels;

public record RocketsVM(IReadOnlyCollection<RocketVM> Rockets, int TotalPagesCount);

// TODO: Move it to Rocket.cs ?
public record RocketVM
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public List<RocketPropertyVM> Properties { get; init; } = new();
}

public record RocketPropertyVM(string Name, string Value, string? Symbol);
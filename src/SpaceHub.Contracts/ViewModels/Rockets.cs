namespace SpaceHub.Contracts.ViewModels;

public record RocketsVM(IReadOnlyList<RocketVM> Rockets, int TotalPagesCount);
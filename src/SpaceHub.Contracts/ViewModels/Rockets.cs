namespace SpaceHub.Contracts.ViewModels;

public record RocketsVM(IReadOnlyCollection<RocketVM> Rockets, int TotalPagesCount);
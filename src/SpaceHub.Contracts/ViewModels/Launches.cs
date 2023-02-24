namespace SpaceHub.Contracts.ViewModels;

public record LaunchesVM(IReadOnlyList<LaunchVM> Launches, int TotalPagesCount);

public class LaunchVM
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Status { get; init; }
    public DateTime? Date { get; init; }
    public required string ImageUrl { get; init; }
    public string? MissionDescription { get; init; }
    public required string AgencyName { get; init; }
    public required string PadLocationName { get; init; }
    public required bool Upcoming { get; init; }
    public TimeSpan? TimeToLaunch { get; set; }
}
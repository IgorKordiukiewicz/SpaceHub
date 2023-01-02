namespace SpaceHub.Contracts.ViewModels;

// TODO: News (and later events too) use the response: IReadOnlyCollection<T> Items, int TotalPagesCount -> Extract it to separate class/record
// maybe use Result<T> for all responses
public record LaunchesVM(IReadOnlyCollection<LaunchVM> Launches, int TotalPagesCount);

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
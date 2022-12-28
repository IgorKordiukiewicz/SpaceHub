namespace SpaceHub.Contracts.ViewModels;

// TODO: News (and later events too) use the response: IReadOnlyCollection<T> Items, int TotalPagesCount -> Extract it to separate class/record
public record LaunchesVM(IReadOnlyCollection<LaunchVM> Launches, int TotalPagesCount);

public class LaunchVM
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Status { get; init; }
    public DateTime? Date { get; init; }
    public string ImageUrl { get; init; }
    public string? MissionDescription { get; init; }
    public string AgencyName { get; init; }
    public string PadLocationName { get; init; }
    public bool Upcoming { get; init; }
    public TimeSpan? TimeToLaunch { get; set; }
}
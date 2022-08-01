namespace Web.ViewModels;

public class LaunchDetailsCardViewModel
{
    public string ApiId { get; init; }
    public string Name { get; init; }
    public string ImageUrl { get; init; }
    public string? Mission { get; init; }
    public string Date { get; init; }
    public long DateJsMilliseconds { get; init; }
    public bool Upcoming { get; init; }
    public string? WindowStart { get; init; }
    public string? WindowEnd { get; init; }
    public string StatusName { get; init; }
    public string StatusDescription { get; init; }
    public string? VideoUrl { get; init; }
    public bool IsSaved { get; set; }
}

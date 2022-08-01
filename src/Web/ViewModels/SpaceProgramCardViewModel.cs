namespace Web.ViewModels;

public record SpaceProgramCardViewModel
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string ImageUrl { get; init; }
    public string? StartDate { get; init; }
    public string? EndDate { get; init; }
    public string? InfoUrl { get; init; }
    public string? WikiUrl { get; init; }
}

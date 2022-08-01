namespace Library.Models;

public record SpaceProgram
{
    public string Name { get; init; }
    public string Description { get; init; }
    public List<Agency> Agencies { get; init; }
    public string ImageUrl { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? InfoUrl { get; init; }
    public string? WikiUrl { get; init; }
}

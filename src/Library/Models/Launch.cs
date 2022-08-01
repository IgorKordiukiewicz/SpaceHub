namespace Library.Models;

public record Launch
{
    public string ApiId { get; init; }
    public string Name { get; init; }
    public string StatusName { get; init; }
    public string StatusDescription { get; init; }
    public string StatusAbbrevation { get; init; }
    public DateTime? Date { get; init; }
    public DateTime? WindowStart { get; init; }
    public DateTime? WindowEnd { get; init; }
    public Agency Agency { get; init; }
    public Rocket Rocket { get; init; }
    public Mission? Mission { get; init; }
    public Pad Pad { get; init; }
    public List<SpaceProgram> Programs { get; init; }
    public string ImageUrl { get; init; }
    public string? VideoUrl { get; init; }
}

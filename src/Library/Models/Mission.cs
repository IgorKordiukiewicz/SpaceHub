namespace Library.Models;

public record Mission
{
    public string Name { get; init; }
    public string Description { get; init; }
    public string? Designator { get; init; }
    public string Type { get; init; }
    public string? OrbitName { get; init; }
    public string? OrbitAbbrevation { get; init; }
}

using Library.Enums;

namespace Library.Models;

public class RocketRankedProperty
{
    public Rocket Rocket { get; init; }
    public RocketRankedPropertyType Type { get; init; }
    public object? Value { get; init; }
    public object? SecondaryValue { get; init; }
}

using SpaceHub.Contracts.Enums;
using System.Text.Json.Serialization;

namespace SpaceHub.Contracts.Utils;

// TODO: Move somewhere else, or rename Utils/ to something else
[JsonDerivedType(typeof(IndividualComparisonGroup), (int)EComparisonGroup.Individual)]
[JsonDerivedType(typeof(FamilyComparisonGroup), (int)EComparisonGroup.Family)]
[JsonDerivedType(typeof(AllComparisonGroup), (int)EComparisonGroup.All)]
public abstract record ComparisonGroup
{
    public abstract EComparisonGroup Type { get; }
    public required Guid Id { get; init; }
}

public record IndividualComparisonGroup : ComparisonGroup
{
    public required int RocketId { get; init; }
    public override EComparisonGroup Type => EComparisonGroup.Individual;
}

public record FamilyComparisonGroup : ComparisonGroup
{
    public required string FamilyName { get; init; }
    public override EComparisonGroup Type => EComparisonGroup.Family;
}

public record AllComparisonGroup : ComparisonGroup
{
    public override EComparisonGroup Type => EComparisonGroup.All;
}

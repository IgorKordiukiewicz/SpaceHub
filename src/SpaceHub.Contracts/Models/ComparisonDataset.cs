using FluentValidation;
using SpaceHub.Contracts.Enums;
using System.Text.Json.Serialization;

namespace SpaceHub.Contracts.Models;

[JsonDerivedType(typeof(IndividualRocketsComparisonDataset), (int)ERocketComparisonDataset.Individual)]
[JsonDerivedType(typeof(FamilyRocketsComparisonDataset), (int)ERocketComparisonDataset.Family)]
[JsonDerivedType(typeof(AllRocketsComparisonDataset), (int)ERocketComparisonDataset.All)]
public abstract record RocketsComparisonDataset
{
    public abstract ERocketComparisonDataset Type { get; }
    public required Guid Id { get; init; }
}

public class RocketComparisonDatasetValidator : AbstractValidator<RocketsComparisonDataset>
{
    public RocketComparisonDatasetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x).SetInheritanceValidator(v =>
        {
            v.Add(new IndividualRocketsComparisonDatasetValidator());
            v.Add(new FamilyRocketsComparisonDatasetValidator());
        });
    }
}

public record IndividualRocketsComparisonDataset : RocketsComparisonDataset
{
    public required int RocketId { get; init; }
    public required string RocketName { get; init; }
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.Individual;
}

public class IndividualRocketsComparisonDatasetValidator : AbstractValidator<IndividualRocketsComparisonDataset>
{
    public IndividualRocketsComparisonDatasetValidator()
    {
        RuleFor(x => x.RocketId).NotEmpty();
        RuleFor(x => x.RocketName).NotEmpty();
    }
}

public record FamilyRocketsComparisonDataset : RocketsComparisonDataset
{
    public required string FamilyName { get; init; }
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.Family;
}

public class FamilyRocketsComparisonDatasetValidator : AbstractValidator<FamilyRocketsComparisonDataset>
{
    public FamilyRocketsComparisonDatasetValidator()
    {
        RuleFor(x => x.FamilyName).NotEmpty();
    }
}

public record AllRocketsComparisonDataset : RocketsComparisonDataset
{
    public override ERocketComparisonDataset Type => ERocketComparisonDataset.All;
}

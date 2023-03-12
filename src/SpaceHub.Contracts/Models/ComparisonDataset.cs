using FluentValidation;

namespace SpaceHub.Contracts.Models;

public record ComparisonDataset
{
    public required Guid Id { get; init; }
    public required int RocketId { get; init; }
    public required string RocketName { get; init; }
}

public class ComparisonDatasetValidator : AbstractValidator<ComparisonDataset>
{
    public ComparisonDatasetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RocketId).NotEmpty();
        RuleFor(x => x.RocketName).NotEmpty();
    }
}

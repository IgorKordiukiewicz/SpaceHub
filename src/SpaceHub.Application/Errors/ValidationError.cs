using FluentValidation.Results;

namespace SpaceHub.Application.Errors;

public class ValidationError : Error
{
    public ValidationError(IReadOnlyCollection<ValidationFailure> validationFailures)
        : base(CreateMessage(validationFailures)) { }

    private static string CreateMessage(IReadOnlyCollection<ValidationFailure> validationFailures)
    {
        var errorMessages = validationFailures.Select(x => x.ErrorMessage);
        return string.Join(";", errorMessages);
    }
}

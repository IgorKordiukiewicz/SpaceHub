using FluentResults;

namespace SpaceHub.Application.Errors;

public class ValidationError : Error
{
    public ValidationError(string message)
        : base(message) { }
}

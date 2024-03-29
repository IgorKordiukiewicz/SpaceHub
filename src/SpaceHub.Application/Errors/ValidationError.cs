﻿using FluentValidation.Results;

namespace SpaceHub.Application.Errors;

public class ValidationError : Error
{
    public ValidationError(IEnumerable<ValidationFailure> validationFailures)
        : base(CreateMessage(validationFailures)) { }

    private static string CreateMessage(IEnumerable<ValidationFailure> validationFailures)
    {
        var errorMessages = validationFailures.Select(x => x.ErrorMessage);
        return string.Join(";", errorMessages);
    }
}

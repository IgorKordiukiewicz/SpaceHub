using FluentResults;

namespace SpaceHub.Application.Errors;

public class RecordNotFoundError : Error
{
    public RecordNotFoundError(string message) 
        : base(message) { }
}

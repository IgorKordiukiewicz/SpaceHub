using FluentResults;
using Refit;

namespace SpaceHub.Infrastructure.Errors;

public class ApiError : Error
{
    public ApiError(string requestUri, string errorMessage)
        : base(CreateMessage(requestUri, errorMessage)) { }

    public ApiError(IApiResponse apiResponse)
        : this(apiResponse.RequestMessage?.RequestUri?.ToString() ?? string.Empty,
              apiResponse.Error?.Message ?? string.Empty)
    { }

    private static string CreateMessage(string requestUri, string errorMessage)
        => $"Request to \"{requestUri}\" failed with message: \"{errorMessage}\"";
}

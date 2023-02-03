﻿using Refit;

namespace SpaceHub.Application.Errors;

public class ApiError : Error
{
    public ApiError(string requestUri, string errorMessage)
        : base(CreateMessage(requestUri, errorMessage)) { }

    public ApiError(IApiResponse apiResponse)
        : this(apiResponse.RequestMessage?.RequestUri?.ToString() ?? string.Empty, 
              apiResponse.Error?.Message ?? string.Empty) { }

    private static string CreateMessage(string requestUri, string errorMessage)
        => $"Request to \"{requestUri}\" failed with message: \"{errorMessage}\"";
}

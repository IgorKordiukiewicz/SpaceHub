﻿using DnsClient.Internal;
using Microsoft.Extensions.Logging;

namespace SpaceHub.Application.Behaviors;

public class ResultLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public ResultLoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if(response is null)
        {
            return response; 
        }

        var errors = typeof(TResponse).GetProperty("Errors")?.GetValue(response) as List<IError>;
        var error = errors?.FirstOrDefault();
        if(error is not null)
        {
            _logger.LogError("Request: \"{request}\" failed with error message: \"{errorMessage}\"", request.ToString(), error.Message);
        }

        return response;
    }
}

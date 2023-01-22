using DnsClient.Internal;
using MediatR;
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

        var isFaulted = typeof(TResponse).GetProperty("IsFaulted")?.GetValue(response) as bool?;
        if(isFaulted is not null && isFaulted.Value)
        {
            _logger.LogError(response.ToString());
        }

        return response;
    }
}

using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SpaceHub.Application.Behaviors;
using SpaceHub.Application.Features.Launches;
using System.Reflection;

namespace SpaceHub.Application;

public static class Application
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Order matters, result logging has to be registered before validation,
        // otherwise result logging will not get called if validation returns failure
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResultLoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

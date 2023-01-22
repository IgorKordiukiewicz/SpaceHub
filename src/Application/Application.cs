using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SpaceHub.Application.Behaviors;
using System.Reflection;

namespace SpaceHub.Application;

public static class Application
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ResultLoggingBehavior<,>));

        return services;
    }
}

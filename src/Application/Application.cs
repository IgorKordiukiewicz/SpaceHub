using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SpaceHub.Application;

public static class Application
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}

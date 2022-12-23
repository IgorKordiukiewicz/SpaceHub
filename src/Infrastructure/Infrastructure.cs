using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SpaceHub.Infrastructure.Api;

namespace SpaceHub.Infrastructure;

public static class Infrastructure
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IArticleApi>().ConfigureHttpClient(x =>
        {
            x.BaseAddress = new Uri(configuration["Api:Article:BaseAddress"]);
        });

        services.AddRefitClient<ILaunchApi>().ConfigureHttpClient(x =>
        {
            x.BaseAddress = new Uri(configuration["Api:Launch:BaseAddress"]);
        });

        return services;
    }
}
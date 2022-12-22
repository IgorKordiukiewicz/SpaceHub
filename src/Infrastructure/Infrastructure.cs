using Infrastructure.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infrastructure;

public static class Infrastructure
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRefitClient<IArticleApi>().ConfigureHttpClient(x =>
        {
            x.BaseAddress = new Uri(configuration["Api:Article:BaseAddress"]);
        });

        return services;
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Services;

namespace SpaceHub.Infrastructure;

public static class Infrastructure
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, InfrastructureSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings, nameof(settings));
        ArgumentException.ThrowIfNullOrEmpty(settings.DatabaseName, nameof(settings.DatabaseName));

        services.AddRefitClient<IArticleApi>().ConfigureHttpClient(x =>
        {
            x.BaseAddress = new Uri(settings.Api.Article.BaseAddress);
        });

        services.AddRefitClient<ILaunchApi>().ConfigureHttpClient(x =>
        {
            x.BaseAddress = new Uri(settings.Api.Launch.BaseAddress);
        });

        services.AddSingleton<DbContext>();
        services.AddScoped<IDataUpdateService, DataUpdateService>();

        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SpaceHub.Application.Interfaces;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Synchronization;
using SpaceHub.Infrastructure.Synchronization.Interfaces;

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
        services.AddSingleton<IDbContext>(factory => factory.GetRequiredService<DbContext>());

        services.AddScoped<IDataSynchronizer<Article>, ArticlesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<Agency>, AgenciesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<Launch>, LaunchesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<Rocket>, RocketsDataSynchronizer>();

        return services;
    }
}
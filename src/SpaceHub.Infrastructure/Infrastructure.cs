using Microsoft.Extensions.DependencyInjection;
using Refit;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
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
        services.AddScoped<IDataSynchronizer<ArticleModel>, ArticlesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<AgencyModel>, AgenciesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<LaunchModel>, LaunchesDataSynchronizer>();
        services.AddScoped<IDataSynchronizer<RocketModel>, RocketsDataSynchronizer>();

        return services;
    }
}
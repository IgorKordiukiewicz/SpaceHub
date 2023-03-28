using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MediatR;
using MongoDB.Driver;
using SpaceHub.Infrastructure;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Synchronization.Interfaces;

namespace SpaceHub.Web.Server;

public static class Hangfire
{
    public static void ConfigureHangfire(this IServiceCollection services, InfrastructureSettings settings)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(settings.ConnectionStrings.MongoDB + "/" + settings.DatabaseName);
        var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                Prefix = "Hangfire",
                CheckConnection = false
            }));

        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = settings.DatabaseName + ".Hangfire";
        });
    }

    public static void AddJobs(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
        var articlesSynchronizer = scope.ServiceProvider.GetRequiredService<IDataSynchronizer<ArticleModel>>();
        var agenciesSynchronizer = scope.ServiceProvider.GetRequiredService<IDataSynchronizer<AgencyModel>>();
        var launchesSynchronizer = scope.ServiceProvider.GetRequiredService<IDataSynchronizer<LaunchModel>>();
        var rocketsSynchronizer = scope.ServiceProvider.GetRequiredService<IDataSynchronizer<RocketModel>>();

        var utcZone = TimeZoneInfo.Utc;
        recurringJobManager.AddOrUpdate(
            "Update articles",
            () => articlesSynchronizer.Synchronize(),
            "0 * * * *", 
            utcZone);
        recurringJobManager.AddOrUpdate(
            "Update launches",
            () => launchesSynchronizer.Synchronize(),
            "30 0,8,16 * * *",
            utcZone);
        recurringJobManager.AddOrUpdate(
            "Update rockets",
            () => rocketsSynchronizer.Synchronize(),
            "0 1 * * *",
            utcZone);
        recurringJobManager.AddOrUpdate(
            "Update agencies",
            () => agenciesSynchronizer.Synchronize(),
            "0 1 * * *",
            utcZone);
    }
}


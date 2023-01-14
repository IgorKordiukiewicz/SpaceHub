using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using MongoDB.Driver;
using SpaceHub.Infrastructure;
using MediatR;
using SpaceHub.Application.Features.News;
using SpaceHub.Application.Features.Launches;

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
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var utcZone = TimeZoneInfo.Utc;
        recurringJobManager.AddOrUpdate(
            "Update articles",
            () => mediator.Send(new UpdateArticlesCommand(), CancellationToken.None),
            "0 * * * *", 
            utcZone);
        recurringJobManager.AddOrUpdate(
            "Update launches",
            () => mediator.Send(new UpdateLaunchesCommand(), CancellationToken.None),
            "30 0,8,16 * * *",
            utcZone);
    }
}


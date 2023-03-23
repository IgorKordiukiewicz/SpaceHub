using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using Xunit;

namespace SpaceHub.IntegrationTests;

public class IntegrationTestsFixture : IDisposable
{
    private readonly IServiceProvider _services;

    public IntegrationTestsFixture()
    {
        var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "InfrastructureSettings:DatabaseName", "SpaceHubTests" }
                });
            });

            builder.UseEnvironment("Development");
        });
        _services = appFactory.Services;
    }

    public void InitDb()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();

        db.Agencies.DeleteMany(Builders<AgencyModel>.Filter.Empty);
        db.Articles.DeleteMany(Builders<ArticleModel>.Filter.Empty);
        db.Launches.DeleteMany(Builders<LaunchModel>.Filter.Empty);
        db.Rockets.DeleteMany(Builders<RocketModel>.Filter.Empty);
        db.CollectionsLastUpdates.DeleteMany(Builders<CollectionLastUpdateModel>.Filter.Empty);
    }

    public void Dispose()
    {
        //
    }
}

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
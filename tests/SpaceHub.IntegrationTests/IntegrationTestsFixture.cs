using Bogus;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using Xunit;

namespace SpaceHub.IntegrationTests;

public class TestDateTimeProvider : IDateTimeProvider
{
    public DateTime Now() => new(2023, 3, 15);
}

public class IntegrationTestsFixture : IDisposable
{
    private readonly IServiceProvider _services;

    public IntegrationTestsFixture()
    {
        var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<InfrastructureSettings>(config =>
                {
                    config.DatabaseName = "SpaceHubTests";
                    config.HangfireEnabled = false;
                });

                services.AddScoped<IDateTimeProvider, TestDateTimeProvider>();
            });

            builder.UseEnvironment("Development");
        });
        _services = appFactory.Services;
    }

    public void ResetDb()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();

        db.Agencies.DeleteMany(Builders<AgencyModel>.Filter.Empty);
        db.Articles.DeleteMany(Builders<ArticleModel>.Filter.Empty);
        db.Launches.DeleteMany(Builders<LaunchModel>.Filter.Empty);
        db.Rockets.DeleteMany(Builders<RocketModel>.Filter.Empty);
        db.CollectionsLastUpdates.DeleteMany(Builders<CollectionLastUpdateModel>.Filter.Empty);
    }

    public void SeedDb(Action<DbContext> action)
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();
        action(db);
    }

    public void Dispose()
    {
        ResetDb();
    }

    public async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(request);
    }

    public async Task<IReadOnlyList<TModel>> GetAsync<TModel>() where TModel : class
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();
        return await db.GetCollection<TModel>().AsQueryable().ToListAsync();
    }

    public async Task<IReadOnlyList<TModel>> GetAsync<TModel>(Func<TModel, bool> predicate) where TModel : class
    {
        return (await GetAsync<TModel>()).Where(predicate).ToList();
    }
}

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
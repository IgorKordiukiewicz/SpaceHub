using Bogus;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SpaceHub.Infrastructure;
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
            builder.ConfigureTestServices(services =>
            {
                services.Configure<InfrastructureSettings>(config =>
                {
                    config.DatabaseName = "SpaceHubTests";
                    config.HangfireEnabled = false;
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
        ResetDbData(db);

        db.Articles.InsertMany(new Faker<ArticleModel>()
            .RuleFor(x => x.PublishDate, f => f.Date.Recent())
            .RuleFor(x => x.Title, f => string.Join(" ", f.Lorem.Words()))
            .RuleFor(x => x.Summary, f => f.Lorem.Word())
            .Generate(15));
    }

    public void Dispose()
    {
        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DbContext>();
        ResetDbData(db);
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

    private static void ResetDbData(DbContext db)
    {
        db.Agencies.DeleteMany(Builders<AgencyModel>.Filter.Empty);
        db.Articles.DeleteMany(Builders<ArticleModel>.Filter.Empty);
        db.Launches.DeleteMany(Builders<LaunchModel>.Filter.Empty);
        db.Rockets.DeleteMany(Builders<RocketModel>.Filter.Empty);
        db.CollectionsLastUpdates.DeleteMany(Builders<CollectionLastUpdateModel>.Filter.Empty);
    }
}

[CollectionDefinition(nameof(IntegrationTestsCollection))]
public class IntegrationTestsCollection : ICollectionFixture<IntegrationTestsFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
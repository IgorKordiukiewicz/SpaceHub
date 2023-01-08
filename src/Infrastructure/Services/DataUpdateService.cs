using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Services;

public class DataUpdateService : BackgroundService
{
    private readonly IArticleApi _articleApi;
    private readonly DbContext _db;
    private readonly PeriodicTimer _timer;

    public DataUpdateService(IArticleApi articleApi, DbContext db, IOptions<InfrastructureSettings> settingsOptions)
    {
        _articleApi = articleApi;
        _db = db;

        _timer = new PeriodicTimer(TimeSpan.FromMinutes(settingsOptions.Value.DataUpdateEveryXMinutes));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(await _timer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            await UpdateArticles();
        }
    }

    private async Task UpdateArticles()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable<CollectionLastUpdateModel>()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow; // TODO: Use some kind of interface for DateTime
        var articles = await _articleApi.GetArticlesPublishedBetweenAsync(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter());
        if(articles is null)
        {
            // TODO: log,
            // method should return Result<T>, if this method's result is bad then maybe add background job in hangfire to retry in e.g. 30minutes
            return;
        }

        if(articles.Any())
        {
            var newArticles = new List<ArticleModel>();
            foreach (var article in articles)
            {
                newArticles.Add(new ArticleModel()
                {
                    Title = article.Title,
                    Summary = article.Summary,
                    ImageUrl = article.ImageUrl,
                    NewsSite = article.NewsSite,
                    PublishDate = article.PublishDate,
                    Url = article.Url,
                });
            }

            await _db.Articles.InsertManyAsync(newArticles);
        }

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Articles,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));
    }
}

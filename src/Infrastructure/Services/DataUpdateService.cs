using MongoDB.Driver;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Services;

public interface IDataUpdateService
{
    Task UpdateArticles();
}

public class DataUpdateService : IDataUpdateService
{
    private readonly IArticleApi _articleApi;
    private readonly DbContext _db;

    public DataUpdateService(IArticleApi articleApi, DbContext db)
    {
        _articleApi = articleApi;
        _db = db;
    }

    public async Task UpdateArticles()
    {
        var lastUpdateTime = _db.CollectionsLastUpdates.AsQueryable<CollectionLastUpdateModel>()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .Single();

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

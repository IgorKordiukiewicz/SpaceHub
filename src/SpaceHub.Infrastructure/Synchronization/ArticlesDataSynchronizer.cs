using FluentResults;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;
using SpaceHub.Infrastructure.Synchronization.Interfaces;

namespace SpaceHub.Infrastructure.Synchronization;

public class ArticlesDataSynchronizer : IDataSynchronizer<ArticleModel>
{
    private readonly DbContext _db;
    private readonly IArticleApi _api;

    public ArticlesDataSynchronizer(DbContext db, IArticleApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Synchronize()
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow;
        var response = await _api.GetArticlesPublishedBetween(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter());
        if (!response.GetContentOrError().TryPickT0(out var articles, out var error))
        {
            return Result.Fail(error);
        }

        if (articles.Any())
        {
            var newArticles = new List<ArticleModel>();
            foreach (var article in articles)
            {
                newArticles.Add(CreateModel(article));
            }

            if (newArticles.Any())
            {
                await _db.Articles.InsertManyAsync(newArticles);
            }
        }

        _ = await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Articles,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));

        return Result.Ok();

        static ArticleModel CreateModel(ArticleResponse response)
        {
            return new ArticleModel()
            {
                Title = response.Title,
                Summary = response.Summary,
                ImageUrl = response.ImageUrl,
                NewsSite = response.NewsSite,
                PublishDate = response.PublishDate,
                Url = response.Url,
            };
        }
    }
}

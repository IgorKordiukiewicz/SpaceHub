using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.News;

public record UpdateArticlesCommand() : IRequest<Result>;

internal class UpdateArticlesHandler : IRequestHandler<UpdateArticlesCommand, Result>
{
    private readonly DbContext _db;
    private readonly IArticleApi _api;

    public UpdateArticlesHandler(DbContext db, IArticleApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Result> Handle(UpdateArticlesCommand request, CancellationToken cancellationToken)
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow;
        var response = await _api.GetArticlesPublishedBetween(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter());
        if(!response.GetContentOrError().TryPickT0(out var articles, out var error))
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

            if(newArticles.Any())
            {
                await _db.Articles.InsertManyAsync(newArticles);
            }
        }

        _ = await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Articles,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));

        return Result.Ok();
    }

    private ArticleModel CreateModel(ArticleResponse response)
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

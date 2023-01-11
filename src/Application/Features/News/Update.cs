using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Api.Responses;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Application.Features.News;

public record UpdateArticlesCommand() : IRequest;

internal class UpdateArticlesHandler : IRequestHandler<UpdateArticlesCommand>
{
    private readonly DbContext _db;
    private readonly IArticleApi _api;

    public UpdateArticlesHandler(DbContext db, IArticleApi api)
    {
        _db = db;
        _api = api;
    }

    public async Task<Unit> Handle(UpdateArticlesCommand request, CancellationToken cancellationToken)
    {
        var lastUpdateTime = await _db.CollectionsLastUpdates.AsQueryable()
            .Where(x => x.CollectionType == ECollection.Articles)
            .Select(x => x.LastUpdate)
            .SingleAsync();

        var now = DateTime.UtcNow; // TODO: Use some kind of interface for DateTime
        var articles = await _api.GetArticlesPublishedBetweenAsync(lastUpdateTime.ToQueryParameter(), now.ToQueryParameter());
        if (articles is null)
        {
            // TODO: log,
            // method should return Result<T>, if this method's result is bad then maybe add background job in hangfire to retry in e.g. 30minutes
            return Unit.Value;
        }

        if (articles.Any())
        {
            var newArticles = new List<ArticleModel>();
            foreach (var article in articles)
            {
                newArticles.Add(CreateModel(article);
            }

            await _db.Articles.InsertManyAsync(newArticles);
        }

        await _db.CollectionsLastUpdates.UpdateOneAsync(
            x => x.CollectionType == ECollection.Articles,
            Builders<CollectionLastUpdateModel>.Update.Set(x => x.LastUpdate, now));

        return Unit.Value;
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

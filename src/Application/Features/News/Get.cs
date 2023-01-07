using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SpaceHub.Application.Common;
using SpaceHub.Contracts.ViewModels;
using SpaceHub.Infrastructure.Api;
using SpaceHub.Infrastructure.Data;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string? SearchValue, int PageNumber, int ItemsPerPage) : IRequest<ArticlesVM>;

internal class GetNewsHandler : IRequestHandler<GetNewsQuery, ArticlesVM>
{
    private readonly IArticleApi _articleApi;
    private readonly IMemoryCache _cache;
    private readonly DbContext _db;

    public GetNewsHandler(IArticleApi articleApi, IMemoryCache cache, DbContext db)
    {
        _articleApi = articleApi;
        _cache = cache;
        _db = db;
    }

    public async Task<ArticlesVM> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        int offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var cacheKey = CacheHelpers.GetCacheKeyForRequestWithPages("articles", request.SearchValue, offset, request.ItemsPerPage);
        var articles = await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return (await _articleApi.GetArticlesAsync(request.SearchValue, request.ItemsPerPage, offset)).Select(a => new ArticleVM
            {
                Title = a.Title,
                Summary = a.Summary,
                ImageUrl = a.ImageUrl,
                NewsSite = a.NewsSite,
                PublishDate = a.PublishDate,
                Url = a.Url
            }).ToList();
        });

        var totalArticlesCount = await _cache.GetOrCreateAsync($"articlesCount_{request.SearchValue}_{request.ItemsPerPage}", async entry =>
        {
            return await _articleApi.GetArticlesCountAsync(request.SearchValue);
        });
        var totalPagesCount = Pagination.GetPagesCount(totalArticlesCount, request.ItemsPerPage);

        return new ArticlesVM(articles, totalPagesCount);
    }
}

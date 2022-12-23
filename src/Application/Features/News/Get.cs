using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SpaceHub.Application.Common;
using SpaceHub.Infrastructure.Api;

namespace SpaceHub.Application.Features.News;

public record GetNewsQuery(string? SearchValue, int PageNumber, int ItemsPerPage) : IRequest<GetNewsResult>;
public record GetNewsResult(List<ArticleViewModel> Articles, int TotalPagesCount);

public class GetNewsHandler : IRequestHandler<GetNewsQuery, GetNewsResult>
{
    private readonly IArticleApi _articleApi;
    private readonly IMemoryCache _cache;

    public GetNewsHandler(IArticleApi articleApi, IMemoryCache cache)
    {
        _articleApi = articleApi;
        _cache = cache;
    }

    public async Task<GetNewsResult> Handle(GetNewsQuery request, CancellationToken cancellationToken)
    {
        int offset = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var cacheKey = CacheHelpers.GetCacheKeyForRequestWithPages("articles", request.SearchValue, offset, request.ItemsPerPage);
        var articles = await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return (await _articleApi.GetArticlesAsync(request.SearchValue, request.ItemsPerPage, offset)).Select(a => new ArticleViewModel
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

        return new GetNewsResult(articles, totalPagesCount);
    }
}

public class ArticleViewModel
{
    public string Title { get; set; }
    public string Summary { get; set; }
    public string ImageUrl { get; set; }
    public string NewsSite { get; set; }
    public DateTime PublishDate { get; set; }
    public string Url { get; set; }
}

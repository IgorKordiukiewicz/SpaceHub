using Application.Common;
using Infrastructure.Api;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Features.News;

public record GetAllNewsQuery(string? SearchValue, int PageNumber, int ItemsPerPage) : IRequest<GetAllNewsResult>;
public record GetAllNewsResult(List<ArticleViewModel> Articles, int TotalPagesCount);

public class GetAllNewsHandler : IRequestHandler<GetAllNewsQuery, GetAllNewsResult>
{
    private readonly IArticleApi _articleApi;
    private readonly IMemoryCache _cache;

    public GetAllNewsHandler(IArticleApi articleApi, IMemoryCache cache)
    {
        _articleApi = articleApi;
        _cache = cache;
    }

    public async Task<GetAllNewsResult> Handle(GetAllNewsQuery request, CancellationToken cancellationToken)
    {
        int start = Pagination.GetOffset(request.PageNumber, request.ItemsPerPage);
        var cacheKey = Helpers.GetCacheKeyForRequestWithPages("articles", request.SearchValue, start, request.ItemsPerPage);
        var articles = await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            return (await _articleApi.GetArticlesAsync(request.SearchValue, request.ItemsPerPage, start)).Select(a => new ArticleViewModel
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

        return new GetAllNewsResult(articles, totalPagesCount);
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

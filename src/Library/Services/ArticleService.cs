using Library.Api;
using Library.Mapping;
using Library.Models;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using Library.Services.Interfaces;

namespace Library.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleApi _articleApi;
    private readonly IMemoryCache _cache;

    public ArticleService(IArticleApi articleApi, IMemoryCache cache)
    {
        _articleApi = articleApi;
        _cache = cache;
    }

    public async Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber, int itemsPerPage)
    {         
        int start = Pagination.GetOffset(pageNumber, itemsPerPage);
        var result = await _cache.GetOrCreateAsync(Helpers.GetCacheKeyForRequestWithPages("articles", searchValue, start, itemsPerPage), async entry =>
        {
            return await _articleApi.GetArticlesAsync(searchValue, itemsPerPage, start);
        });

        return result.Select(a => a.ToModel()).ToList();
    }

    public async Task<Article> GetArticleAsync(int id)
    {
        var result = await _cache.GetOrCreateAsync("article" + id.ToString(), async entry =>
        {
            return await _articleApi.GetArticleAsync(id);
        });

        return result.ToModel();
    }

    public async Task<int> GetPagesCountAsync(string? searchValue, int itemsPerPage)
    {
        var articlesCount = await _cache.GetOrCreateAsync($"articlesCount_{searchValue}_{itemsPerPage}", async entry =>
        {
            return await _articleApi.GetArticlesCountAsync(searchValue);
        });

        return Pagination.GetPagesCount(articlesCount, itemsPerPage);
    }
}

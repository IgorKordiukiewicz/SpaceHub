using Library.Api.Responses;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using Library.Services.Interfaces;

namespace Library.Services;

public static class Helpers
{
    public static async Task<(int, T)> GetApiResponseWithSearchAndPagination<T>(string cacheKey, Func<string?, int, int, Task<T>> func, 
        string? searchValue, int pageNumber, int itemsPerPage, IMemoryCache cache)
        where T : MultiElementResponse
    {
        var offset = Pagination.GetOffset(pageNumber, itemsPerPage);
        var result = await cache.GetOrCreateAsync(GetCacheKeyForRequestWithPages(cacheKey, searchValue, offset, itemsPerPage), async entry =>
        {
            return await func(searchValue, itemsPerPage, offset);
        });
        var pagesCount = Pagination.GetPagesCount(result.Count, itemsPerPage);

        return (pagesCount, result);
    }

    public static string GetCacheKeyForRequestWithPages(string name, string? searchValue, int offset, int itemsPerPage)
    {
        return $"{name}_{searchValue}_{offset}_{itemsPerPage}";
    }
}

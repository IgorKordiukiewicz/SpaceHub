using Library.Api.Responses;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public static class Helpers
    {
        public static async Task<(int, T)> GetApiResponseWithSearchAndPagination<T>(string cacheKey, Func<string?, int, int, Task<T>> func, 
            string? searchValue, int pageNumber, Pagination pagination, IMemoryCache cache)
            where T : MultiElementResponse
        {
            var offset = pagination.GetOffset(pageNumber);
            var result = await cache.GetOrCreateAsync(cacheKey + searchValue + pageNumber.ToString(), async entry =>
            {
                return await func(searchValue, pagination.ItemsPerPage, offset);
            });
            var pagesCount = pagination.GetPagesCount(result.Count);

            return (pagesCount, result);
        }
    }
}

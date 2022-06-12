using FluentValidation;
using Library.Api;
using Library.Mapping;
using Library.Models;
using Library.Utils;
using Microsoft.Extensions.Caching.Memory;
using OneOf;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleApi _articleApi;
        private readonly IMemoryCache _cache;

        public Pagination Pagination { get; } = new() { ItemsPerPage = 10 };

        public ArticleService(IArticleApi articleApi, IMemoryCache cache)
        {
            _articleApi = articleApi;
            _cache = cache;
        }

        public async Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber = 1)
        {         
            int start = Pagination.GetOffset(pageNumber);
            var result = await _cache.GetOrCreateAsync("articles" + searchValue + pageNumber.ToString(), async entry =>
            {
                return await _articleApi.GetArticlesAsync(searchValue, Pagination.ItemsPerPage, start);
            });

            return result.Select(a => a.ToModel()).ToList();
        }

        public async Task<int> GetPagesCountAsync(string? searchValue)
        {
            var articlesCount = await _cache.GetOrCreateAsync("articlesCount" + searchValue, async entry =>
            {
                return await _articleApi.GetArticlesCountAsync(searchValue);
            });

            return Pagination.GetPagesCount(articlesCount);
        }
    }
}

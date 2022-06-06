using FluentValidation;
using Library.Api;
using Library.Mapping;
using Library.Models;
using Library.Utils;
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
        private readonly Pagination _pagination = new() { ItemsPerPage = 10 };

        public ArticleService(IArticleApi articleApi)
        {
            _articleApi = articleApi;
        }

        public async Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber = 1)
        {         
            int start = _pagination.GetOffset(pageNumber);
            var result = await _articleApi.GetArticlesAsync(searchValue, _pagination.ItemsPerPage, start);

            return result.Select(a => a.ToModel()).ToList();
        }

        public async Task<int> GetPagesCountAsync(string? searchValue)
        {
            var articlesCount = await _articleApi.GetArticlesCountAsync(searchValue);

            return _pagination.GetPagesCount(articlesCount);
        }
    }
}

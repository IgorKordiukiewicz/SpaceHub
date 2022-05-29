using Library.Api;
using Library.Api.Responses;
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

        private const int _articlesPerPage = 10;

        public ArticleService(IArticleApi articleApi)
        {
            _articleApi = articleApi;
        }

        public async Task<List<ArticleResponse>> GetArticlesAsync(string? searchValue, int pageNumber = 1)
        {
            int start = (pageNumber - 1) * 10;
            return await _articleApi.GetArticlesAsync(searchValue, start);
        }

        public async Task<int> GetPagesCount(string? searchValue)
        {
            var articlesCount = await _articleApi.GetArticlesCountAsync(searchValue);
            return (articlesCount - 1) / _articlesPerPage + 1;
        }
    }
}

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

        public ArticleService(IArticleApi articleApi)
        {
            _articleApi = articleApi;
        }

        public async Task<List<ArticleResponse>> GetArticlesAsync()
        {
            return await _articleApi.GetArticlesAsync();
        }
    }
}

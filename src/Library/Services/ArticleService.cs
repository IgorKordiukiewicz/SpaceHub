using FluentValidation;
using Library.Api;
using Library.Api.Requests;
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
        private readonly IValidator<ArticleRequest> _validator;

        private const int ArticlesPerPage = 10;

        public ArticleService(IArticleApi articleApi, IValidator<ArticleRequest> validator)
        {
            _articleApi = articleApi;
            _validator = validator;
        }

        public async Task<List<ArticleResponse>> GetArticlesAsync(ArticleRequest articleRequest)
        {
            var validationResult = _validator.Validate(articleRequest);
            if(!validationResult.IsValid)
            {
                return new List<ArticleResponse>();
            }
            
            int start = (articleRequest.PageNumber - 1) * ArticlesPerPage;
            return await _articleApi.GetArticlesAsync(articleRequest.SearchValue, start);
        }

        public async Task<int> GetPagesCountAsync(string? searchValue)
        {
            var articlesCount = await _articleApi.GetArticlesCountAsync(searchValue);
            return (articlesCount - 1) / ArticlesPerPage + 1;
        }
    }
}

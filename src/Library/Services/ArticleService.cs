using FluentValidation;
using Library.Api;
using Library.Api.Requests;
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
        private readonly IValidator<ArticleRequest> _validator;
        private readonly Pagination _pagination = new() { ItemsPerPage = 10 };

        public ArticleService(IArticleApi articleApi, IValidator<ArticleRequest> validator)
        {
            _articleApi = articleApi;
            _validator = validator;
        }

        public async Task<List<Article>> GetArticlesAsync(ArticleRequest articleRequest)
        {
            var validationResult = _validator.Validate(articleRequest);
            if(!validationResult.IsValid)
            {
                return new List<Article>();
            }
            
            int start = _pagination.GetOffset(articleRequest.PageNumber);
            var result = await _articleApi.GetArticlesAsync(articleRequest.SearchValue, start);

            return result.Select(a => a.ToModel()).ToList();
        }

        public async Task<int> GetPagesCountAsync(string? searchValue)
        {
            var articlesCount = await _articleApi.GetArticlesCountAsync(searchValue);

            return _pagination.GetPagesCount(articlesCount);
        }
    }
}

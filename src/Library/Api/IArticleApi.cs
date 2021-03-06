using Library.Api.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api
{
    public interface IArticleApi
    {
        [Get("/articles/?_title_contains={searchValue}&_limit={limit}&_start={start}")]
        Task<IEnumerable<ArticleResponse>> GetArticlesAsync(string? searchValue, int limit, int start = 0);

        [Get("/articles/{id}")]
        Task<ArticleResponse> GetArticleAsync(int id);

        [Get("/articles/count/?_title_contains={searchValue}")]
        Task<int> GetArticlesCountAsync(string? searchValue);
    }
}

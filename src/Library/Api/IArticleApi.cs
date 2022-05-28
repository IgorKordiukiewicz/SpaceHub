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
        [Get("/articles")]
        Task<List<ArticleResponse>> GetArticlesAsync();
    }
}

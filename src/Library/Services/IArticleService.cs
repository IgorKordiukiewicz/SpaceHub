using Library.Api.Requests;
using Library.Models;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetArticlesAsync(ArticleRequest articleRequest);

        Task<int> GetPagesCountAsync(string? searchValue);
    }
}

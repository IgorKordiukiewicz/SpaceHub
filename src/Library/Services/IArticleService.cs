using Library.Api.Responses;
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
        Task<List<ArticleResponse>> GetArticlesAsync(string? searchValue);
    }
}

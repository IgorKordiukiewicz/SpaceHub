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
        Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber = 1);

        Task<int> GetPagesCountAsync(string? searchValue);
    }
}

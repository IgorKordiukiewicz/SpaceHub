using Library.Models;
using Library.Utils;
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
        Task<List<Article>> GetArticlesAsync(string? searchValue, int pageNumber = 1, int itemsPerPage = 10);

        Task<Article> GetArticleAsync(int id);

        Task<int> GetPagesCountAsync(string? searchValue, int itemsPerPage = 10);
    }
}

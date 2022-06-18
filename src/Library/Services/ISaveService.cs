using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public interface ISaveService
    {
        Task SaveArticleAsync(Article article);
        Task UnsaveArticleAsync(int articleId);
        Task<bool> IsArticleSavedAsync(int articleId);
        List<Article> GetSavedArticles(int pageNumber = 1);
        int GetSavedArticlesPagesCount();
    }
}

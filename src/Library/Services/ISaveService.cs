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
        bool IsArticleSaved(int articleId);
        List<Article> GetSavedArticles(int pageNumber = 1, int itemsPerPage = 10);
        int GetSavedArticlesPagesCount(int itemsPerPage = 10);

        Task SaveLaunchAsync(Launch launch);
        Task UnsaveLaunchAsync(string launchId);
        bool IsLaunchSaved(string launchId);
        List<Launch> GetSavedLaunches(int pageNumber = 1, int itemsPerPage = 12);
        int GetSavedLaunchesPagesCount(int itemsPerPage = 12);
    }
}

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
        Task SaveArticleAsync(string userId, Article article);
        Task UnsaveArticleAsync(string userId, int articleId);
        bool IsArticleSaved(string userId, int articleId);
        List<Article> GetSavedArticles(string userId, int pageNumber = 1, int itemsPerPage = 10);
        int GetSavedArticlesPagesCount(string userId, int itemsPerPage = 10);

        Task SaveLaunchAsync(string userId, Launch launch);
        Task UnsaveLaunchAsync(string userId, string launchId);
        bool IsLaunchSaved(string userId, string launchId);
        List<Launch> GetSavedLaunches(string userId, int pageNumber = 1, int itemsPerPage = 12);
        int GetSavedLaunchesPagesCount(string userId, int itemsPerPage = 12);
    }
}

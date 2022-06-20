using Library.Data;
using Library.Mapping;
using Library.Models;
using Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Services
{
    public class SaveService : ISaveService
    {
        private readonly AppDbContext _context;

        public SaveService(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveArticleAsync(string userId, Article article)
        {
            var articleEntity = article.ToEntity();
            articleEntity.UserId = userId;

            await _context.Articles.AddAsync(articleEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UnsaveArticleAsync(string userId, int articleId)
        {
            var articleEntity = _context.Articles.FirstOrDefault(a => a.UserId == userId && a.ApiId == articleId);
            if(articleEntity != null)
            {
                _context.Articles.Remove(articleEntity);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsArticleSaved(string userId, int articleId)
        {
            return _context.Articles.Any(a => a.UserId == userId && a.ApiId == articleId);
        }

        public List<Article> GetSavedArticles(string userId, int pageNumber, int itemsPerPage)
        {
            int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
            return _context.Articles
                .Where(a => a.UserId == userId)
                .Select(a => a.ToModel())
                .Skip(offset)
                .Take(itemsPerPage)
                .ToList();
        }

        public int GetSavedArticlesPagesCount(string userId, int itemsPerPage)
        {
            return Pagination.GetPagesCount(_context.Articles.Where(a => a.UserId == userId).Count(), itemsPerPage);
        }

        public async Task SaveLaunchAsync(string userId, Launch launch)
        {
            var launchEntity = launch.ToEntity();
            launchEntity.UserId = userId;

            await _context.Launches.AddAsync(launchEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UnsaveLaunchAsync(string userId, string launchId)
        {
            var launchEntity = _context.Launches.FirstOrDefault(l => l.UserId == userId && l.ApiId == launchId);
            if(launchEntity != null)
            {
                _context.Launches.Remove(launchEntity);
                await _context.SaveChangesAsync();
            }
        }

        public bool IsLaunchSaved(string userId, string launchId)
        {
            return _context.Launches.Any(l => l.UserId == userId && l.ApiId == launchId);
        }

        public List<Launch> GetSavedLaunches(string userId, int pageNumber, int itemsPerPage)
        {
            int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
            return _context.Launches
                .Where(l => l.UserId == userId)
                .Select(l => l.ToModel())
                .Skip(offset)
                .Take(itemsPerPage)
                .ToList();
        }

        public int GetSavedLaunchesPagesCount(string userId, int itemsPerPage)
        {
            return Pagination.GetPagesCount(_context.Launches.Where(l => l.UserId == userId).Count(), itemsPerPage);
        }
    }
}

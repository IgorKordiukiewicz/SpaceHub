using Library.Data;
using Library.Mapping;
using Library.Models;
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

        public async Task SaveArticleAsync(Article article)
        {
            var articleEntity = article.ToEntity();

            await _context.Articles.AddAsync(articleEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UnsaveArticleAsync(int articleId)
        {
            var articleEntity = await _context.Articles.FindAsync(articleId);
            if(articleEntity != null)
            {
                _context.Articles.Remove(articleEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsArticleSavedAsync(int articleId)
        {
            var result = await _context.Articles.FindAsync(articleId);
            return result != null;
        }
    }
}

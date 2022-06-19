﻿using Library.Data;
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

        public bool IsArticleSaved(int articleId)
        {
            return _context.Articles.Any(a => a.ApiId == articleId);
        }

        public List<Article> GetSavedArticles(int pageNumber, int itemsPerPage)
        {
            int offset = Pagination.GetOffset(pageNumber, itemsPerPage);
            return _context.Articles.Select(a => a.ToModel()).Skip(offset).Take(itemsPerPage).ToList();
        }

        public int GetSavedArticlesPagesCount(int itemsPerPage)
        {
            return Pagination.GetPagesCount(_context.Articles.Count(), itemsPerPage);
        }
    }
}

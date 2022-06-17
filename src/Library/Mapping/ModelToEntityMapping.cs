using Library.Data.Entities;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping
{
    public static class ModelToEntityMapping
    {
        public static ArticleEntity ToEntity(this Article article)
        {
            return new()
            {
                ApiId = article.ApiId,
                Title = article.Title,
                Summary = article.Summary,
                Url = article.Url,
                ImageUrl = article.ImageUrl,
                NewsSite = article.NewsSite,
                PublishDate = article.PublishDate,
                UpdateDate = article.UpdateDate
            };
        }
    }
}

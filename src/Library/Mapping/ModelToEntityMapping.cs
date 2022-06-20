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

        public static LaunchEntity ToEntity(this Launch launch)
        {
            return new()
            {
                ApiId = launch.ApiId,
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                AgencyName = launch.Agency.Name,
                PadName = launch.Pad.Name,
                Status = launch.StatusName,
                Date = launch.Date,
            };
        }

        public static EventEntity ToEntity(this Event event_)
        {
            return new()
            {
                ApiId = event_.ApiId,
                Name = event_.Name,
                Type = event_.Type,
                Description = event_.Description,
                Location = event_.Location,
                ImageUrl = event_.ImageUrl,
                Date = event_.Date,
            };
        }
    }
}

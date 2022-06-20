using Library.Data.Entities;
using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Mapping
{
    public static class EntityToModelMapping
    {
        public static Article ToModel(this ArticleEntity article)
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

        public static Launch ToModel(this LaunchEntity launch)
        {
            return new()
            {
                ApiId = launch.ApiId,
                Name = launch.Name,
                ImageUrl = launch.ImageUrl,
                Agency = new()
                {
                    Name = launch.AgencyName,
                },
                Pad = new()
                {
                    Name = launch.PadName,
                },
                StatusName = launch.Status,
                Date = launch.Date,
            };
        }

        public static Event ToModel(this EventEntity event_)
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

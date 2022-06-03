using Library.Api.Responses;
using System.Globalization;

namespace Web.ViewModels
{
    public record ArticleViewModel
    {
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public string ImageUrl { get; init; }
        public string NewsSite { get; init; }
        public string PublishDate { get; init; }

        public ArticleViewModel(ArticleResponse article)
        {
            Title = article.Title;
            Summary = article.Summary;
            Url = article.Url;
            ImageUrl = article.ImageUrl;
            NewsSite = article.NewsSite;
            PublishDate = Utils.DateToString(article.PublishDate, true);
        }
    }
}

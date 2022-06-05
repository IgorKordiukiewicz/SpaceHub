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
    }
}

using Library.Api.Responses;

namespace Web.ViewModels
{
    public record ArticleViewModel
    {
        public string Title { get; init; }
        public string Summary { get; init; }
        public string Url { get; init; }
        public string ImageUrl { get; init; }

        public ArticleViewModel(ArticleResponse articleResponse)
        {
            Title = articleResponse.Title;
            Summary = articleResponse.Summary;
            Url = articleResponse.Url;
            ImageUrl = articleResponse.ImageUrl;
        }
    }
}

namespace Web.ViewModels;

public class ArticleCardViewModel
{
    public int ApiId { get; init; }
    public string Title { get; init; }
    public string Summary { get; init; }
    public string Url { get; init; }
    public string ImageUrl { get; init; }
    public string NewsSite { get; init; }
    public string PublishDate { get; init; }
    public bool IsSaved { get; set; }
}

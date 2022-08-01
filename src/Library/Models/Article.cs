namespace Library.Models;

public record Article
{
    public int ApiId { get; init; }
    public string Title { get; init; }
    public string Summary { get; init; }
    public string Url { get; init; }
    public string ImageUrl { get; init; }
    public string NewsSite { get; init; }
    public DateTime PublishDate { get; init; }
    public DateTime UpdateDate { get; init; }
}

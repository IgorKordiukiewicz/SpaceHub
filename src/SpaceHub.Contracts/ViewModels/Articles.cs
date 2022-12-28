namespace SpaceHub.Contracts.ViewModels;

public record ArticlesVM(IReadOnlyCollection<ArticleVM> Articles, int TotalPagesCount);

// TODO: Handle the nullable warnings
public record ArticleVM
{
    public string Title { get; init; }
    public string Summary { get; init; }
    public string ImageUrl { get; init; }
    public string NewsSite { get; init; }
    public DateTime PublishDate { get; init; }
    public string Url { get; init; }
}

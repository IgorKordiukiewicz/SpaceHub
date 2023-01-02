namespace SpaceHub.Contracts.ViewModels;

public record ArticlesVM(IReadOnlyCollection<ArticleVM> Articles, int TotalPagesCount);

// TODO: Handle the nullable warnings
public record ArticleVM
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string ImageUrl { get; init; }
    public required string NewsSite { get; init; }
    public required DateTime PublishDate { get; init; }
    public required string Url { get; init; }
}

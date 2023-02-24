namespace SpaceHub.Contracts.ViewModels;

public record ArticlesVM(IReadOnlyList<ArticleVM> Articles, int TotalPagesCount);

public record ArticleVM
{
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string ImageUrl { get; init; }
    public required string NewsSite { get; init; }
    public required DateTime PublishDate { get; init; }
    public required string Url { get; init; }
}

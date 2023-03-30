namespace SpaceHub.Domain.Models;

public class Article
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string ImageUrl { get; init; }
    public required string NewsSite { get; init; }
    public required DateTime PublishDate { get; init; }
    public required string Url { get; init; }
}

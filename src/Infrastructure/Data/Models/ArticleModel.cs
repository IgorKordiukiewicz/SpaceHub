using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SpaceHub.Infrastructure.Data.Models;

public class ArticleModel
{
    [BsonId]
    public required ObjectId Id { get; init; }
    public required string Title { get; init; }
    public required string Summary { get; init; }
    public required string ImageUrl { get; init; }
    public required string NewsSite { get; init; }
    public required DateTime PublishDate { get; init; }
    public required string Url { get; init; }
}

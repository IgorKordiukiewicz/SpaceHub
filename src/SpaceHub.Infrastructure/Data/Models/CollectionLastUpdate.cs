using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Data.Models;

public class CollectionLastUpdate
{
    [BsonId]
    public ObjectId Id { get; init; }
    public required ECollection CollectionType { get; init; }
    public required DateTime LastUpdate { get; set; }
}

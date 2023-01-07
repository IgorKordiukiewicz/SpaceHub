using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceHub.Infrastructure.Enums;

namespace SpaceHub.Infrastructure.Data.Models;

public class CollectionLastUpdateModel
{
    [BsonId]
    public ObjectId Id { get; init; }
    public required ECollection CollectionType { get; init; }
    public required DateTime LastUpdate { get; set; }
}

using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using SpaceHub.Application.Interfaces;
using SpaceHub.Domain.Models;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Infrastructure.Data;

public class DbContext : IDbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly Dictionary<Type, string> _collectionsNamesByType = new()
    {
        { typeof(Article), nameof(Articles) },
        { typeof(Launch), nameof(Launches) },
        { typeof(Rocket), nameof(Rockets) },
        { typeof(Agency), nameof(Agencies) },
        { typeof(CollectionLastUpdate), nameof(CollectionsLastUpdates) },
    };

    public IMongoCollection<Article> Articles { get; set; }
    public IMongoCollection<Launch> Launches { get; set; }
    public IMongoCollection<Rocket> Rockets { get; set; }
    public IMongoCollection<Agency> Agencies { get; set; }
    public IMongoCollection<CollectionLastUpdate> CollectionsLastUpdates { get; set; }

    public DbContext(IOptions<InfrastructureSettings> settingsOptions)
    {
        RegisterClassMaps();

        var settings = settingsOptions.Value;
        var connectionString = settings.ConnectionStrings.MongoDB ?? throw new ArgumentNullException(nameof(settings.ConnectionStrings.MongoDB));
        var dbName = settings.DatabaseName ?? throw new ArgumentNullException(nameof(settings.DatabaseName));

        _client = new MongoClient(connectionString);
        _db = _client.GetDatabase(dbName);

        Articles = _db.GetCollection<Article>(_collectionsNamesByType[typeof(Article)]);
        Launches = _db.GetCollection<Launch>(_collectionsNamesByType[typeof(Launch)]);
        Rockets = _db.GetCollection<Rocket>(_collectionsNamesByType[typeof(Rocket)]);
        Agencies = _db.GetCollection<Agency>(_collectionsNamesByType[typeof(Agency)]);
        CollectionsLastUpdates = _db.GetCollection<CollectionLastUpdate>(_collectionsNamesByType[typeof(CollectionLastUpdate)]);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class
    {
        if(!_collectionsNamesByType.TryGetValue(typeof(TDocument), out var collectionName))
        {
            throw new ArgumentOutOfRangeException(typeof(TDocument).ToString(), "Collection with this document type does not exist.");
        }

        return _db.GetCollection<TDocument>(collectionName);
    }

    private static void RegisterClassMaps()
    {
        BsonClassMap.RegisterClassMap<Article>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.Id).SetIdGenerator(CombGuidGenerator.Instance);
        });

        BsonClassMap.RegisterClassMap<Launch>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.ApiId);
        });

        BsonClassMap.RegisterClassMap<Agency>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.ApiId);
        });

        BsonClassMap.RegisterClassMap<Rocket>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.ApiId);
            cm.MapMember(x => x.LiftoffThrust).SetElementName("ThrustAtLiftoff");
            cm.UnmapMember(x => x.CostPerKgToGeo);
            cm.UnmapMember(x => x.CostPerKgToLeo);
            cm.UnmapMember(x => x.LaunchSuccess);
        });
    }
}

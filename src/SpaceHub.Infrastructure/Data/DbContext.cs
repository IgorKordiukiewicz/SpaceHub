using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Infrastructure.Data;

public class DbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly Dictionary<Type, string> _collectionsNamesByType = new()
    {
        { typeof(ArticleModel), nameof(Articles) },
        { typeof(LaunchModel), nameof(Launches) },
        { typeof(RocketModel), nameof(Rockets) },
        { typeof(AgencyModel), nameof(Agencies) },
        { typeof(CollectionLastUpdateModel), nameof(CollectionsLastUpdates) },
    };

    public IMongoCollection<ArticleModel> Articles { get; set; }
    public IMongoCollection<LaunchModel> Launches { get; set; }
    public IMongoCollection<RocketModel> Rockets { get; set; }
    public IMongoCollection<AgencyModel> Agencies { get; set; }
    public IMongoCollection<CollectionLastUpdateModel> CollectionsLastUpdates { get; set; }

    public DbContext(IOptions<InfrastructureSettings> settingsOptions)
    {
        var settings = settingsOptions.Value;
        var connectionString = settings.ConnectionStrings.MongoDB ?? throw new ArgumentNullException(nameof(settings.ConnectionStrings.MongoDB));
        var dbName = settings.DatabaseName ?? throw new ArgumentNullException(nameof(settings.DatabaseName));

        _client = new MongoClient(connectionString);
        _db = _client.GetDatabase(dbName);

        Articles = _db.GetCollection<ArticleModel>(_collectionsNamesByType[typeof(ArticleModel)]);
        Launches = _db.GetCollection<LaunchModel>(_collectionsNamesByType[typeof(LaunchModel)]);
        Rockets = _db.GetCollection<RocketModel>(_collectionsNamesByType[typeof(RocketModel)]);
        Agencies = _db.GetCollection<AgencyModel>(_collectionsNamesByType[typeof(AgencyModel)]);
        CollectionsLastUpdates = _db.GetCollection<CollectionLastUpdateModel>(_collectionsNamesByType[typeof(CollectionLastUpdateModel)]);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class
    {
        if(!_collectionsNamesByType.TryGetValue(typeof(TDocument), out var collectionName))
        {
            throw new ArgumentOutOfRangeException(typeof(TDocument).ToString(), "Collection with this document type does not exist.");
        }

        return _db.GetCollection<TDocument>(collectionName);
    }
}

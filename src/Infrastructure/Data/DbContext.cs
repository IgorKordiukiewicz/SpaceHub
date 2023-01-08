﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Infrastructure.Data;

public class DbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _db;

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

        Articles = _db.GetCollection<ArticleModel>("Articles");
        Launches = _db.GetCollection<LaunchModel>("Launches");
        Rockets = _db.GetCollection<RocketModel>("Rockets");
        Agencies = _db.GetCollection<AgencyModel>("Agencies");
        CollectionsLastUpdates = _db.GetCollection<CollectionLastUpdateModel>("CollectionsLastUpdates");
    }
}

using MongoDB.Driver;
using SpaceHub.Infrastructure.Data.Models;

namespace SpaceHub.Infrastructure.Data;

public class DbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _db;

    public IMongoCollection<ArticleModel> Articles { get; set; }

    public DbContext(string connectionString, string dbName)
    {
        ArgumentNullException.ThrowIfNull(connectionString, nameof(connectionString));
        ArgumentNullException.ThrowIfNull(dbName, nameof(dbName));

        _client = new MongoClient(connectionString);
        _db = _client.GetDatabase(dbName);

        Articles = _db.GetCollection<ArticleModel>("Articles");
    }
}

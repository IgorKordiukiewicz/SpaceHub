using SpaceHub.Domain.Models;

namespace SpaceHub.Application.Interfaces;

public interface IDbContext
{
    IMongoCollection<Article> Articles { get; set; }
    IMongoCollection<Launch> Launches { get; set; }
    IMongoCollection<Rocket> Rockets { get; set; }
    IMongoCollection<Agency> Agencies { get; set; }

    IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class;
}

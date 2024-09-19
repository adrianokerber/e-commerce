using MongoDB.Driver;

namespace OrderManagement.Infrastructure.Database.MongoDB;

public sealed class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoDatabase database) => _database = database;

    public IMongoCollection<T> GetCollection<T>(string collectionName)
        => _database.GetCollection<T>(collectionName);
}
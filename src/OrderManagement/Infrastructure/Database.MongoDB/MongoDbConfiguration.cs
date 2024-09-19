namespace OrderManagement.Infrastructure.Database.MongoDB;

public record MongoDbConfiguration(
    string DatabaseName,
    string ConnectionString
);
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using OrderManagement.Domain.Orders.Infrastructure.MongoDbMappers;
using OrderManagement.Infrastructure.Database.MongoDB;
using OrderManagement.Infrastructure.Http;

namespace OrderManagement.Api.StartupInfra;

internal static class ServicesExtensions
{
    public static IServiceCollection AddHttpGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<HttpGlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
    
    public static IServiceCollection AddOpenApiSpecs(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApiDocument();
        return services;
    }
    
    public static IServiceCollection AddSingleDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbConfiguration = GetMongoDbConfiguration(configuration);

        var client = new MongoClient(mongoDbConfiguration.ConnectionString);
        var database = client.GetDatabase(mongoDbConfiguration.DatabaseName);
        var mongoDbContext = new MongoDbContext(database);

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, t => true);
        
        // Register each Aggregate mapping:
        OrderDbMapper.RegisterMappings();

        services.AddSingleton(mongoDbContext);

        return services;
    }

    private static MongoDbConfiguration GetMongoDbConfiguration(IConfiguration configuration)
    {
        var mongoDbConfiguration = configuration
            .GetSection(nameof(MongoDbConfiguration))
            .Get<MongoDbConfiguration>();
        
        if (mongoDbConfiguration is null
            || string.IsNullOrEmpty(mongoDbConfiguration.ConnectionString)
            || string.IsNullOrEmpty(mongoDbConfiguration.DatabaseName))
        {
            throw new MongoConfigurationException($"Invalid '{nameof(MongoDbConfiguration)}' definition!");
        }

        return mongoDbConfiguration;
    }
}
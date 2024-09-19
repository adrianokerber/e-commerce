using MongoDB.Bson.Serialization;
using OrderManagement.Domain.Shared.ValueObjects;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders.Infrastructure.MongoDbMappers;

public static class OrderDbMapper
{
    public static void RegisterMappings()
    {
        BsonClassMap.RegisterClassMap<Order>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            
            cm.MapProperty("State").SetElementName("state");
            cm.MapProperty("Items").SetElementName("items");
            cm.MapProperty("CreatedOn").SetElementName("created_on");
            cm.MapProperty("PaymentMethod").SetElementName("payment_method");
            cm.MapProperty("CustomerEmail").SetElementName("customer_email");
        });
        
        BsonClassMap.RegisterClassMap<Email>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            
            cm.MapProperty("Value").SetElementName("value");
        });
    }
}
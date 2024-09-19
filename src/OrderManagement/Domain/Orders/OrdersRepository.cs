using CSharpFunctionalExtensions;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderManagement.Infrastructure.Database.MongoDB;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders;

public sealed class OrdersRepository : IService<OrdersRepository>
{
    private readonly IMongoCollection<Order> _collection;
    
    public OrdersRepository(MongoDbContext mongoDbContext)
    {
        _collection = mongoDbContext.GetCollection<Order>("Orders");
    }

    public async Task AddOrder(Order order, CancellationToken ct = default)
    {
        await _collection.InsertOneAsync(order, ct);
    }

    public async Task<Maybe<Order>> FindOrderById(Guid id, CancellationToken ct = default)
    {
        var result = await _collection.FindAsync(order => order.Id == id);
        return await result.FirstOrDefaultAsync(ct)
                           .AsMaybe();
    }
    
    public async Task<Result> UpdateOrderState(Order order)
    {
        var filterDef = Builders<Order>.Filter.Eq(o => o.Hash, order.Hash);
        var updateDef = Builders<Order>.Update.Set(o => o.Hash, Guid.NewGuid())
                                                                  .Set(o => o.PreviousStates, order.PreviousStates);
        var result = await _collection.UpdateOneAsync(filterDef, updateDef);
        
        if (!result.IsAcknowledged && result.ModifiedCount <= 0)
            return Result.Failure("Failed to update order");
        
        return Result.Success();
    }
    
    // TODO: we must use FindOneAndUpdate({ id: Id }, { $inc: { useCount: 1 });
}
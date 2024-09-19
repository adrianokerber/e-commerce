using CSharpFunctionalExtensions;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders.Application.PlaceOrder;

public sealed class DiscountApplierService : IService<DiscountApplierService>
{
    public Result<decimal> ApplyDiscounts(IEnumerable<IDiscount> discounts, decimal subtotal)
        => discounts.Aggregate(subtotal, (current, discount) => current <= 0m ? 0m : discount.Apply(current));
}
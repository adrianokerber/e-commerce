using CSharpFunctionalExtensions;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders.Application.PlaceOrder;

public sealed class DiscountSelectorService : IService<DiscountSelectorService>
{
    private readonly List<IDiscount> _discounts =
    [
        new TotalItemsQuantityDiscount(5, 3.5m),
        new TotalItemsQuantityDiscount(50, 15m),
        new QuantityPerItemDiscount("IPHONE16-256GB-CHROMA", 2, 200m),
        new ChristmasDiscount(.2m)
    ];
    
    public IEnumerable<IDiscount> SelectDiscounts(List<OrderItem> orderItems, DateTimeOffset date)
    {
        foreach (var discount in _discounts)
        {
            if (discount.Match(orderItems, date))
                yield return discount;
        }
    }
}

public interface IDiscount
{
    bool Match(List<OrderItem> orderItems, DateTimeOffset date);
    decimal Apply(decimal subtotal);
}

public class TotalItemsQuantityDiscount : IDiscount
{
    public int ExpectedQuantity { get; }
    public decimal DiscountInBrl { get; }
    
    public TotalItemsQuantityDiscount(int expectedQuantity, decimal discountInBrl)
    {
        ExpectedQuantity = expectedQuantity;
        DiscountInBrl = discountInBrl;
    }

    public bool Match(List<OrderItem> orderItems, DateTimeOffset date)
    {
        var totalItems = orderItems.Aggregate(0, (current, orderItem) => current + orderItem.Quantity);
        return totalItems == 5;
    }

    public decimal Apply(decimal subtotal) => subtotal - DiscountInBrl;
}

public class QuantityPerItemDiscount : IDiscount
{
    public string Sku { get; }
    public int ExpectedQuantity { get; }
    public decimal DiscountInBrl { get; }
    
    public QuantityPerItemDiscount(string sku, int expectedQuantity, decimal discountInBrl)
    {
        Sku = sku;
        ExpectedQuantity = expectedQuantity;
        DiscountInBrl = discountInBrl;
    }

    public bool Match(List<OrderItem> orderItems, DateTimeOffset date)
    {
        var specificProduct = orderItems.FirstOrDefault(x => x.Sku == Sku)
                                                       .AsMaybe();
        return specificProduct.HasValue
            && specificProduct.Value.Quantity == ExpectedQuantity;
    }

    public decimal Apply(decimal subtotal) => subtotal - DiscountInBrl;
}

public class ChristmasDiscount : IDiscount
{
    private decimal PercentageDiscount { get; }

    public ChristmasDiscount(decimal percentageDiscount)
    {
        PercentageDiscount = percentageDiscount;
    }
    
    public bool Match(List<OrderItem> orderItems, DateTimeOffset date)
    {
        return date.Date is { Day: 25, Month: 12 };
    }

    public decimal Apply(decimal subtotal) => subtotal - (subtotal * PercentageDiscount);
}
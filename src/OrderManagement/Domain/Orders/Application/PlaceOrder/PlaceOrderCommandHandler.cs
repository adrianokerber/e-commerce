using CSharpFunctionalExtensions;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders.Application.PlaceOrder;

public class PlaceOrderCommandHandler : CommandHandler<PlaceOrderCommand, Order>, IService<PlaceOrderCommandHandler>
{
    private readonly OrdersRepository _ordersRepository;
    private readonly DiscountSelectorService _discountSelectorService;
    private readonly DiscountApplierService _discountApplierService;

    public PlaceOrderCommandHandler(OrdersRepository ordersRepository, DiscountSelectorService discountSelectorService, DiscountApplierService discountApplierService)
    {
        _ordersRepository = ordersRepository;
        _discountSelectorService = discountSelectorService;
        _discountApplierService = discountApplierService;
    }

    public override async Task<Result<Order>> HandleAsync(PlaceOrderCommand command, CancellationToken ct = default)
    {
        var subtotal = CalculateSubtotal(command.OrderItems); // TODO: migrate logic to Order
        var discounts = _discountSelectorService.SelectDiscounts(command.OrderItems, command.Date);
        var total = _discountApplierService.ApplyDiscounts(discounts, subtotal); // TODO: migrate logic to Order
        
        var order = Order.Create(Guid.NewGuid(),
                                            command.Email,
                                            command.OrderItems,
                                            command.PaymentMethod,
                                            command.Date);
        if (order.IsFailure)
            return Result.Failure<Order>(order.Error);

        await _ordersRepository.AddOrder(order.Value);

        return order;
    }

    private decimal CalculateSubtotal(List<OrderItem> orderItems)
    {
        return orderItems.Aggregate(0m, (current, orderItem) => current + (orderItem.UnitPrice * orderItem.Quantity));
    }
}
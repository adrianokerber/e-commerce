using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Orders.Application.CancelOrder;

public class CancelOrderCommand
{
    public Guid OrderId { get; }

    private CancelOrderCommand(Guid orderId)
    {
        OrderId = orderId;
    }
    
    public static Result<CancelOrderCommand> Create(Guid orderId)
    {
        if (orderId == Guid.Empty)
            return Result.Failure<CancelOrderCommand>("OrderId cannot be empty");

        return new CancelOrderCommand(orderId);
    }
}
using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Orders.OrderStates;

public class PendingState : State
{
    public override string Status { get; } = "Aguardando processamento";

    public override Result CancelOrder()
    {
        _order.TransitionTo(new CanceledState());
        return Result.Success();
    }

    public override Result ProcessPayment()
    {
        _order.TransitionTo(new ProcessingPaymentState());
        
        // Call Payment Gateway?
        
        return Result.Success();
    }
}
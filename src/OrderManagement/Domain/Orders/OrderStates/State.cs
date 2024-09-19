using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Orders.OrderStates;

public abstract class State
{
    protected Order _order;
    public abstract string Status { get; }

    public void SetOrder(Order order)
    {
        _order = order;
    }

    public abstract Result CancelOrder();
    public abstract Result ProcessPayment();
}
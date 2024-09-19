using CSharpFunctionalExtensions;
using OrderManagement.Domain.Orders.OrderStates;
using OrderManagement.Domain.Shared.ValueObjects;

namespace OrderManagement.Domain.Orders;

public sealed class Order : Entity<Guid>
{
    public Guid Hash { get; init; } = Guid.NewGuid();
    private State State
    {
        get => PreviousStates.Last();
        set
        {
            value.SetOrder(this);
            PreviousStates = PreviousStates.Append(value);
        }
    }

    public IEnumerable<State> PreviousStates { get; private set; } = Enumerable.Empty<State>();
    private Email CustomerEmail { get; }
    private List<OrderItem> Items { get; }
    private DateTimeOffset CreatedOn { get; }
    private PaymentMethod PaymentMethod { get; }
    
    // TODO: review if prices should be static here or updated each change
    // private decimal SubtotalPrice { get; }
    // private decimal Discount { get; }
    // private decimal TotalPrice { get; }
    
    private Order(Guid id, Email email, List<OrderItem> items, PaymentMethod paymentMethod, DateTimeOffset createdOn, State state)
    {
        Id = id;
        CustomerEmail = email;
        Items = items;
        PaymentMethod = paymentMethod;
        CreatedOn = createdOn;
        TransitionTo(state);
    }

    public static Result<Order> Create(Guid id, Email email, List<OrderItem> items, PaymentMethod paymentMethod, DateTimeOffset createdOn)
    {
        if (id == Guid.Empty)
            return Result.Failure<Order>($"{nameof(id)} cannot be empty");
        if (items.Any() != true)
            return Result.Failure<Order>($"{nameof(items)} cannot be empty");
        if (items.Any(item => item.UnitPrice < 0 || item.Quantity <= 0 || string.IsNullOrWhiteSpace(item.Sku)))
            return Result.Failure<Order>($"All {nameof(items)} must contain positive values to quantity and unitPrice. Also the SKU cannot be empty");
        if (createdOn == default)
            return Result.Failure<Order>($"{nameof(createdOn)} cannot be empty");

        return new Order(id, email, items, paymentMethod, createdOn, new PendingState());
    }
    
    /// <summary>
    /// Warning: this method should be called only by a concrete state itself not client code 
    /// </summary>
    /// <param name="state"></param>
    public void TransitionTo(State state)
    {
        State = state;
        
        // TODO: send email to client? Send Domain event?
    }
    
    public Result Cancel()
    {
        return State.CancelOrder();
    }

    public Result ProcessPayment()
    {
        return State.ProcessPayment();
    }
}

public sealed record OrderItem(string Sku, decimal UnitPrice, int Quantity);
using CSharpFunctionalExtensions;
using OrderManagement.Domain.Shared.ValueObjects;

namespace OrderManagement.Domain.Orders.Application.PlaceOrder;

public class PlaceOrderCommand
{
    public Email Email { get; }
    public string ShippingAddress { get; }
    public List<OrderItem> OrderItems { get; }
    public PaymentMethod PaymentMethod { get; }
    public DateTimeOffset Date { get; }

    private PlaceOrderCommand(Email email, string shippingAddress, List<OrderItem> orderItems, PaymentMethod paymentMethod, DateTimeOffset date)
    {
        Email = email;
        ShippingAddress = shippingAddress;
        OrderItems = orderItems;
        PaymentMethod = paymentMethod;
        Date = date;
    }
    
    public static Result<PlaceOrderCommand> Create(string email, string shippingAddress, List<OrderItem> orderItems, PaymentMethod paymentMethod, DateTimeOffset date)
    {
        var emailCreation = Email.Create(email);
        if (emailCreation.IsFailure)
            return Result.Failure<PlaceOrderCommand>(emailCreation.Error);
        if (string.IsNullOrWhiteSpace(shippingAddress))
            return Result.Failure<PlaceOrderCommand>("Shipping address cannot be empty");
        if (orderItems == null || orderItems.Count == 0)
            return Result.Failure<PlaceOrderCommand>("OrderItems cannot be empty");
        if (date == default)
            return Result.Failure<PlaceOrderCommand>("Date must be initialized");

        return new PlaceOrderCommand(emailCreation.Value, shippingAddress, orderItems, paymentMethod, date);
    }
}
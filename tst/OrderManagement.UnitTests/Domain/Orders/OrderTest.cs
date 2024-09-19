using FluentAssertions;
using OrderManagement.Domain.Orders;
using OrderManagement.Domain.Shared.ValueObjects;

namespace OrderManagement.UnitTests.Domain.Orders;

public class OrderTest
{
    [Fact]
    [Trait("Scenario", "Success")]
    public void OrderCreateMethod_ShouldReturnNewOrder()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        var orderItems = new List<OrderItem>
        {
            new OrderItem("P-1", 10m, 1),
            new OrderItem("P-2", 1m, 2)
        };
        var email = Email.Create("valid@e.mail").Value;
        var paymentMethod = PaymentMethod.PIX;
        var createdOn = DateTimeOffset.Now;
        
        // Act
        var result = Order.Create(orderId, email, orderItems, paymentMethod, createdOn);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should()
                    .Match<Order>(o => o.Id.Equals(orderId));
    }
}
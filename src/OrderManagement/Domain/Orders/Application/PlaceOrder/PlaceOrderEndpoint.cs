using FastEndpoints;
using OrderManagement.Domain.Shared.ValueObjects;
using OrderManagement.Infrastructure.Http;

namespace OrderManagement.Domain.Orders.Application.PlaceOrder;

public class PlaceOrderEndpoint : Endpoint<Request, object>
{
    private readonly PlaceOrderCommandHandler _commandHandler;
    private readonly HttpResponseFactory _httpResponseFactory;

    public PlaceOrderEndpoint(PlaceOrderCommandHandler commandHandler, HttpResponseFactory httpResponseFactory)
    {
        _commandHandler = commandHandler;
        _httpResponseFactory = httpResponseFactory;
    }

    public override void Configure()
    {
        Post("/orders/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = PlaceOrderCommand.Create(req.Email, req.ShippingAddress, req.Items, req.PaymentMethod, DateTime.UtcNow);
        if (command.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateErrorWith400("Invalid request", command.Error));
            return;
        }
        
        var result = await _commandHandler.HandleAsync(command.Value);
        if (result.IsFailure)
        {
            await SendResultAsync(_httpResponseFactory.CreateErrorWith400("Invalid order", result.Error));
            return;
        }

        await SendResultAsync(_httpResponseFactory.CreateSuccessWith200(result.Value));
    }
}

public record struct Request(string Email,
                             string ShippingAddress,
                             List<OrderItem> Items,
                             PaymentMethod PaymentMethod);
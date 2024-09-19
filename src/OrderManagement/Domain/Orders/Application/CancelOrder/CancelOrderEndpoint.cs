using FastEndpoints;
using OrderManagement.Domain.Orders.Application.PlaceOrder;
using OrderManagement.Infrastructure.Http;

namespace OrderManagement.Domain.Orders.Application.CancelOrder;

public class CancelOrderEndpoint : Endpoint<Request, object>
{
    private readonly CancelOrderCommandHandler _commandHandler;
    private readonly HttpResponseFactory _httpResponseFactory;

    public CancelOrderEndpoint(CancelOrderCommandHandler commandHandler, HttpResponseFactory httpResponseFactory)
    {
        _commandHandler = commandHandler;
        _httpResponseFactory = httpResponseFactory;
    }

    public override void Configure()
    {
        Post("/orders/{id}/cancel"); // TODO: find out why the route parameter ID is not configured properly
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var command = CancelOrderCommand.Create(req.OrderId);
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

public record struct Request(Guid OrderId);
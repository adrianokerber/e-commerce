using CSharpFunctionalExtensions;
using OrderManagement.Shared;

namespace OrderManagement.Domain.Orders.Application.CancelOrder;

public class CancelOrderCommandHandler : CommandHandler<CancelOrderCommand, string>, IService<CancelOrderCommandHandler>
{
    private readonly OrdersRepository _ordersRepository;

    public CancelOrderCommandHandler(OrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public override async Task<Result<string>> HandleAsync(CancelOrderCommand command, CancellationToken ct = default)
    {
        var order = await _ordersRepository.FindOrderById(command.OrderId);
        if (order.HasNoValue)
            return Result.Failure<string>($"Order with id '{command.OrderId}' not found");

        var result = order.Value.Cancel();
        if (result.IsFailure)
            return Result.Failure<string>(result.Error);
        
        var updateResult = await _ordersRepository.UpdateOrderState(order.Value);
        if (updateResult.IsFailure)
            return Result.Failure<string>(updateResult.Error);

        return "Ordem Cancelada";
    }
}
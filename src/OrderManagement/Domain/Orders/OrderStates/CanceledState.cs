using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Orders.OrderStates;

public class CanceledState : State
{
    public override string Status { get; } = "Cancelado";
    public override Result CancelOrder()
    {
        return Result.Failure("Ordem já cancelada");
    }
}
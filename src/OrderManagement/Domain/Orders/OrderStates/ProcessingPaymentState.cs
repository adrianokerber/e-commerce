using CSharpFunctionalExtensions;

namespace OrderManagement.Domain.Orders.OrderStates;

public sealed class ProcessingPaymentState : State
{
    public override string Status { get; } = "Processando Pagamento";
    public override Result CancelOrder()
    {
        throw new NotImplementedException();
    }

    public override Result ProcessPayment()
    {
        throw new NotImplementedException();
    }

    // TODO: keep the actions here
}
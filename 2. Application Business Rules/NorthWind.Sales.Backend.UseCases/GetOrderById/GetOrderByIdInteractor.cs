using NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;

namespace NorthWind.Sales.Backend.UseCases.GetOrderById;

internal sealed class GetOrderByIdInteractor(
    IQueriesRepository repository,
    IGetOrderByIdOutputPort outputPort) : IGetOrderByIdInputPort
{
    public async Task GetOrderById(int orderId)
    {
        var invoice = await repository.GetOrderById(orderId);
        await outputPort.Handle(invoice);
    }
}

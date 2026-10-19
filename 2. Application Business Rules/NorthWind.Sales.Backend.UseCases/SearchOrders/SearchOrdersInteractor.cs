using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;

namespace NorthWind.Sales.Backend.UseCases.SearchOrders;

internal sealed class SearchOrdersInteractor(
    IQueriesRepository repository,
    ISearchOrdersOutputPort outputPort) : ISearchOrdersInputPort
{
    public async Task SearchOrders(string? filter)
    {
        var orders = await repository.SearchOrders(filter);
        await outputPort.Handle(orders);
    }
}

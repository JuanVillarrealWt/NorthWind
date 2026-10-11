using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;
using NorthWind.Sales.Entities.Dtos.SearchOrders;

namespace NorthWind.Sales.Backend.Presenters.SearchOrders;

internal sealed class SearchOrdersPresenter : ISearchOrdersOutputPort
{
    public IEnumerable<OrderHeaderDto> Orders { get; private set; } = [];

    public Task Handle(IEnumerable<OrderHeaderDto> orders)
    {
        Orders = orders;
        return Task.CompletedTask;
    }
}

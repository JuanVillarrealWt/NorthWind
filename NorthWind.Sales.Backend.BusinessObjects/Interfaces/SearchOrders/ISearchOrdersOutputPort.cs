using NorthWind.Sales.Entities.Dtos.SearchOrders;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;

public interface ISearchOrdersOutputPort
{
    IEnumerable<OrderHeaderDto> Orders { get; }
    Task Handle(IEnumerable<OrderHeaderDto> orders);
}

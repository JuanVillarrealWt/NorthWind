using NorthWind.Sales.Entities.Dtos.SearchOrders;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchOrdersGateway
{
    Task<IEnumerable<OrderHeaderDto>> SearchOrders(string? filter);
    Task<OrderInvoiceDto?> GetOrderById(int orderId);
}

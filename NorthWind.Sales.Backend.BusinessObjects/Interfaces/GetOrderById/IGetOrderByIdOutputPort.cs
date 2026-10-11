using NorthWind.Sales.Entities.Dtos.SearchOrders;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;

public interface IGetOrderByIdOutputPort
{
    OrderInvoiceDto? Invoice { get; }
    Task Handle(OrderInvoiceDto? invoice);
}

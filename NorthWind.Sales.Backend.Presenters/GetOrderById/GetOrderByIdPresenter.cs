using NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;
using NorthWind.Sales.Entities.Dtos.SearchOrders;

namespace NorthWind.Sales.Backend.Presenters.GetOrderById;

internal sealed class GetOrderByIdPresenter : IGetOrderByIdOutputPort
{
    public OrderInvoiceDto? Invoice { get; private set; }

    public Task Handle(OrderInvoiceDto? invoice)
    {
        Invoice = invoice;
        return Task.CompletedTask;
    }
}

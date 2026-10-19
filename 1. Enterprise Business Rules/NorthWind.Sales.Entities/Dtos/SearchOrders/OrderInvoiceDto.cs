namespace NorthWind.Sales.Entities.Dtos.SearchOrders;

public class OrderInvoiceDto
{
    public OrderHeaderDto Header { get; set; } = null!;
    public List<OrderDetailItemDto> Details { get; set; } = [];
}

namespace NorthWind.Sales.Entities.Dtos.SearchOrders;

public class OrderDetailItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public decimal Subtotal => UnitPrice * Quantity;
}

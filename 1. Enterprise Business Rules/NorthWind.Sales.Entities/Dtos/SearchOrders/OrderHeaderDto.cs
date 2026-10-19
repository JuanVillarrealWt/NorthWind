namespace NorthWind.Sales.Entities.Dtos.SearchOrders;

public class OrderHeaderDto
{
    public int Id { get; set; }
    public string CustomerId { get; set; } = null!;
    public string CustomerName { get; set; } = null!;
    public string SalespersonName { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string ShipAddress { get; set; } = null!;
    public string ShipCity { get; set; } = null!;
    public string ShipCountry { get; set; } = null!;
    public string ShipPostalCode { get; set; } = null!;
    public decimal Total { get; set; }
}

namespace NorthWind.Sales.Entities.Dtos.SearchCustomers;

public class CustomerDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal CurrentBalance { get; set; }
}

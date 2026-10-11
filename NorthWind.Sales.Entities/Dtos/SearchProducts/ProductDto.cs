namespace NorthWind.Sales.Entities.Dtos.SearchProducts;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
}

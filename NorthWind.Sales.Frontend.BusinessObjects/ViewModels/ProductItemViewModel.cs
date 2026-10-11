namespace NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

public class ProductItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
}

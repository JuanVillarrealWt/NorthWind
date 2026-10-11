namespace NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

public class CustomerItemViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal CurrentBalance { get; set; }
}

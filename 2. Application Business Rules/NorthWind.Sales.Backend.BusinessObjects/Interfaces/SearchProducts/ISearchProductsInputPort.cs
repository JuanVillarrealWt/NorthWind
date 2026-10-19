namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;

public interface ISearchProductsInputPort
{
    Task SearchProducts(string? filter);
}

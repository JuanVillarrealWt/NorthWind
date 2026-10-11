using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface ISearchProductsGateway
{
    Task<IEnumerable<ProductItemViewModel>> SearchProducts(string? filter);
}

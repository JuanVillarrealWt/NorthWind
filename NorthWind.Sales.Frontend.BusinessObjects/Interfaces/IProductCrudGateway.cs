using NorthWind.Sales.Frontend.BusinessObjects.ViewModels;

namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

public interface IProductCrudGateway
{
    Task CreateProduct(ProductItemViewModel product);
    Task UpdateProduct(ProductItemViewModel product);
    Task DeleteProduct(int id);
}

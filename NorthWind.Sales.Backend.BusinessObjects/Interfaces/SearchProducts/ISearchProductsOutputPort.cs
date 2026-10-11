using NorthWind.Sales.Entities.Dtos.SearchProducts;

namespace NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;

public interface ISearchProductsOutputPort
{
    IEnumerable<ProductDto> Products { get; }
    Task Handle(IEnumerable<ProductDto> products);
}

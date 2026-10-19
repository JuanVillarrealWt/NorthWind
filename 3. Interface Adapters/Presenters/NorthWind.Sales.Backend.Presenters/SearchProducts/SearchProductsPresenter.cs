using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;
using NorthWind.Sales.Entities.Dtos.SearchProducts;

namespace NorthWind.Sales.Backend.Presenters.SearchProducts;

internal sealed class SearchProductsPresenter : ISearchProductsOutputPort
{
    public IEnumerable<ProductDto> Products { get; private set; } = [];

    public Task Handle(IEnumerable<ProductDto> products)
    {
        Products = products;
        return Task.CompletedTask;
    }
}

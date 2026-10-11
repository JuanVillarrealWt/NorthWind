using NorthWind.Sales.Backend.BusinessObjects.Interfaces.Repositories;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;

namespace NorthWind.Sales.Backend.UseCases.SearchProducts;

internal sealed class SearchProductsInteractor(
    IQueriesRepository repository,
    ISearchProductsOutputPort outputPort) : ISearchProductsInputPort
{
    public async Task SearchProducts(string? filter)
    {
        var products = await repository.SearchProducts(filter);
        await outputPort.Handle(products);
    }
}

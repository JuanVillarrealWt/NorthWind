using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;
using NorthWind.Sales.Entities.Dtos.SearchProducts;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchProductsController
{
    public static WebApplication UseSearchProductsController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchProducts, SearchProducts)
            .RequireAuthorization();

        return app;
    }

    public static async Task<IEnumerable<ProductDto>> SearchProducts(
        string? filter,
        ISearchProductsInputPort inputPort,
        ISearchProductsOutputPort presenter)
    {
        await inputPort.SearchProducts(filter);
        return presenter.Products;
    }
}

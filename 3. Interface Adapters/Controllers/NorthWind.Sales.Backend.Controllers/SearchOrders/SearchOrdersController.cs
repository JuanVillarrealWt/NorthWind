using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;
using NorthWind.Sales.Entities.Dtos.SearchOrders;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchOrdersController
{
    public static WebApplication UseSearchOrdersController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchOrders, SearchOrders)
            .RequireAuthorization();

        return app;
    }

    public static async Task<IEnumerable<OrderHeaderDto>> SearchOrders(
        string? filter,
        ISearchOrdersInputPort inputPort,
        ISearchOrdersOutputPort presenter)
    {
        await inputPort.SearchOrders(filter);
        return presenter.Orders;
    }
}

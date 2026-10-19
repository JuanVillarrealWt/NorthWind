using NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;
using NorthWind.Sales.Entities.Dtos.SearchOrders;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class GetOrderByIdController
{
    public static WebApplication UseGetOrderByIdController(this WebApplication app)
    {
        app.MapGet(Endpoints.GetOrderById, GetOrderById)
            .RequireAuthorization();

        return app;
    }

    public static async Task<OrderInvoiceDto?> GetOrderById(
        int id,
        IGetOrderByIdInputPort inputPort,
        IGetOrderByIdOutputPort presenter)
    {
        await inputPort.GetOrderById(id);
        return presenter.Invoice;
    }
}

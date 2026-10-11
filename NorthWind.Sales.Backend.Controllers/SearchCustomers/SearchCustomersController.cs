using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;
using NorthWind.Sales.Entities.Dtos.SearchCustomers;
using NorthWind.Sales.Entities.ValueObjects;

namespace Microsoft.AspNetCore.Builder;

public static class SearchCustomersController
{
    public static WebApplication UseSearchCustomersController(this WebApplication app)
    {
        app.MapGet(Endpoints.SearchCustomers, SearchCustomers)
            .RequireAuthorization();

        return app;
    }

    public static async Task<IEnumerable<CustomerDto>> SearchCustomers(
        string? filter,
        ISearchCustomersInputPort inputPort,
        ISearchCustomersOutputPort presenter)
    {
        await inputPort.SearchCustomers(filter);
        return presenter.Customers;
    }
}

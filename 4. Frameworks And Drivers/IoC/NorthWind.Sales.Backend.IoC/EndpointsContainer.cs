namespace Microsoft.AspNetCore.Builder;

public static class EndpointsContainer
{
    public static WebApplication MapNorthWindSalesEndpoints(this WebApplication app)
    {
        app.UseCreateOrderController();
        app.UseSearchCustomersController();
        app.UseSearchProductsController();
        app.UseSearchOrdersController();
        app.UseGetOrderByIdController();
        app.UseCustomerCrudController();
        app.UseProductCrudController();
        app.UseMembershipEndpoints();
        return app;
    }
}

using NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;
using NorthWind.Sales.Backend.Presenters.GetOrderById;
using NorthWind.Sales.Backend.Presenters.SearchCustomers;
using NorthWind.Sales.Backend.Presenters.SearchOrders;
using NorthWind.Sales.Backend.Presenters.SearchProducts;

namespace Microsoft.Extensions.DependencyInjection;

public static class PresenterDependencyContainer
{
    public static IServiceCollection AddPresenters(
        this IServiceCollection services)
    {
        services.AddScoped<
            ICreateOrderOutputPort,
            NorthWind.Sales.Backend.Presenters.CreateOrder.CreateOrderPresenter>();
        services.AddScoped<
            ISearchCustomersOutputPort,
            SearchCustomersPresenter>();
        services.AddScoped<
            ISearchProductsOutputPort,
            SearchProductsPresenter>();
        services.AddScoped<
            ISearchOrdersOutputPort,
            SearchOrdersPresenter>();
        services.AddScoped<
            IGetOrderByIdOutputPort,
            GetOrderByIdPresenter>();

        return services;
    }
}

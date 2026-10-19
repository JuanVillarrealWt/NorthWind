using NorthWind.Sales.Backend.BusinessObjects.Interfaces.GetOrderById;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchCustomers;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchOrders;
using NorthWind.Sales.Backend.BusinessObjects.Interfaces.SearchProducts;
using NorthWind.Sales.Backend.UseCases.CreateOrder;
using NorthWind.Sales.Backend.UseCases.GetOrderById;
using NorthWind.Sales.Backend.UseCases.SearchCustomers;
using NorthWind.Sales.Backend.UseCases.SearchOrders;
using NorthWind.Sales.Backend.UseCases.SearchProducts;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services)
    {
        services.AddScoped<
            ICreateOrderInputPort,
            CreateOrderInteractor>();
        services.AddScoped<
            ISearchCustomersInputPort,
            SearchCustomersInteractor>();
        services.AddScoped<
            ISearchProductsInputPort,
            SearchProductsInteractor>();
        services.AddScoped<
            ISearchOrdersInputPort,
            SearchOrdersInteractor>();
        services.AddScoped<
            IGetOrderByIdInputPort,
            GetOrderByIdInteractor>();

        services.AddModelValidator<CreateOrderDto, CreateOrderCustomerValidator>();
        services.AddModelValidator<CreateOrderDto, CreateOrderProductValidator>();
        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>, SendEMailWhenSpecialOrderCreatedEventHandler>();

        return services;
    }
}

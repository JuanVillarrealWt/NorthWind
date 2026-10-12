namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddViewsServices(this IServiceCollection services)
    {
        services.AddAppNotifications();
        services.AddScoped<NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder.CreateOrderViewModel>();
        services.AddModelValidator<NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder.CreateOrderViewModel, NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder.CreateOrderViewModelValidator>();
        return services;
    }
}

using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.WebApiGateways;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddWebApiGateways(
        this IServiceCollection services,
        Action<HttpClient> configureHttpClient,
        Action<IHttpClientBuilder> configureHttpClientBuilder)
    {
        services.AddExceptionDelegatingHandler();

        var builder1 = services.AddHttpClient<ICreateOrderGateway, CreateOrderGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder1);

        var builder2 = services.AddHttpClient<ISearchCustomersGateway, SearchCustomersGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder2);

        var builder3 = services.AddHttpClient<ISearchProductsGateway, SearchProductsGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder3);

        var builder4 = services.AddHttpClient<ISearchOrdersGateway, SearchOrdersGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder4);

        var builder5 = services.AddHttpClient<ICustomerCrudGateway, CustomerCrudGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder5);

        var builder6 = services.AddHttpClient<IProductCrudGateway, ProductCrudGateway>(configureHttpClient)
            .AddHttpMessageHandler<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>();
        configureHttpClientBuilder(builder6);

        return services;
    }
}

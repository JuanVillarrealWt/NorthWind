namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddDataContexts(
        this IServiceCollection services,
        Action<NorthWind.Sales.Backend.DataContexts.EFCore.Options.DBOptions> configureDBOptions)
    {
        services.Configure(configureDBOptions);
        services.AddScoped<
            NorthWind.Sales.Backend.Repositories.Interfaces.INorthWindSalesCommandsDataContext,
            NorthWind.Sales.Backend.DataContexts.EFCore.Services.NorthWindSalesCommandsDataContext>();
        services.AddScoped<
            NorthWind.Sales.Backend.Repositories.Interfaces.INorthWindSalesQueriesDataContext,
            NorthWind.Sales.Backend.DataContexts.EFCore.Services.NorthWindSalesQueriesDataContext>();
        services.AddScoped<
            NorthWind.Sales.Backend.Repositories.Interfaces.INorthWindDomainLogsDataContext,
            NorthWind.Sales.Backend.DataContexts.EFCore.Services.NorthWindDomainLogsDataContext>();

        return services;
    }
}

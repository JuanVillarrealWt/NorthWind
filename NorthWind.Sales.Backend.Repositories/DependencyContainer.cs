
namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<
            ICommandsRepository,
            NorthWind.Sales.Backend.Repositories.Repositories.CommandsRepository>();
        services.AddScoped<IQueriesRepository, NorthWind.Sales.Backend.Repositories.Repositories.QueriesRepository>();
        services.AddScoped<IDomainLogsRepository, NorthWind.Sales.Backend.Repositories.Repositories.DomainLogsRepository>();

        return services;
    }
}

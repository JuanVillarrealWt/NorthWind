using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal sealed class DatabaseMigrationHostedService(IServiceScopeFactory scopeFactory)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();

        var salesContext = scope.ServiceProvider.GetRequiredService<NorthWindContext>();
        await salesContext.Database.MigrateAsync(cancellationToken);

        var domainLogsContext = scope.ServiceProvider.GetRequiredService<NorthWindDomainLogsContext>();
        await domainLogsContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

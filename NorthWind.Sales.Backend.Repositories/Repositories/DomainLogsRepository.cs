namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal sealed class DomainLogsRepository(INorthWindDomainLogsDataContext context)
    : IDomainLogsRepository
{
    public async Task Add(NorthWind.DomainLogs.Entities.ValueObjects.DomainLog log) =>
        await context.AddLogAsync(new NorthWind.Sales.Backend.Repositories.Entities.DomainLog
        {
            CreatedDate = log.DateTime,
            Information = log.Information,
            UserName = log.UserName
        });

    public async Task SaveChanges() => await context.SaveChangesAsync();
}

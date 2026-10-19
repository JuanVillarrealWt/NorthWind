namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

internal sealed class NorthWindDomainLogsDataContext(IOptions<DBOptions> dbOptions)
    : NorthWindDomainLogsContext(dbOptions), INorthWindDomainLogsDataContext
{
    public async Task AddLogAsync(NorthWind.Sales.Backend.Repositories.Entities.DomainLog log) =>
        await AddAsync(log);

    public async Task SaveChangesAsync() => await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}

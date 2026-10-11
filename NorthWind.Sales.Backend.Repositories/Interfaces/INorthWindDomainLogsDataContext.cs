namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindDomainLogsDataContext
{
    Task AddLogAsync(NorthWind.Sales.Backend.Repositories.Entities.DomainLog log);
    Task SaveChangesAsync();
}

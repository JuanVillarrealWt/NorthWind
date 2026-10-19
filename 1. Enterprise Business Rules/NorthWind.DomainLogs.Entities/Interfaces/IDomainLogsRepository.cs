namespace NorthWind.DomainLogs.Entities.Interfaces;

public interface IDomainLogsRepository : NorthWind.Entities.Interfaces.IUnitOfWork
{
    Task Add(DomainLog log);
}

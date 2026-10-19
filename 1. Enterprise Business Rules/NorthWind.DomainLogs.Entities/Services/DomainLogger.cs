namespace NorthWind.DomainLogs.Entities.Services;

internal sealed class DomainLogger(IDomainLogsRepository repository) : IDomainLogger
{
    public async Task LogInformation(DomainLog log)
    {
        await repository.Add(log);
        await repository.SaveChanges();
    }
}

namespace NorthWind.DomainLogs.Entities.ValueObjects;

public sealed class DomainLog(string information, string userName)
{
    public DateTime DateTime => DateTime.Now;
    public string Information => information;
    public string UserName => userName;
}

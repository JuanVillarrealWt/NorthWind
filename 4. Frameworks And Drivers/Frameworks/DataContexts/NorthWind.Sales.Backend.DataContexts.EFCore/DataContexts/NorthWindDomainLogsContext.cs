namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

public class NorthWindDomainLogsContext(IOptions<DBOptions> dbOptions) : DbContext
{
    public DbSet<NorthWind.Sales.Backend.Repositories.Entities.DomainLog> DomainLogs { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(dbOptions.Value.DomainLogsConnectionString);
    }
}

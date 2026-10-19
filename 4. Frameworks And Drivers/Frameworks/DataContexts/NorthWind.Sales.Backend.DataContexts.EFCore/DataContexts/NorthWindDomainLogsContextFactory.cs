using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

public sealed class NorthWindDomainLogsContextFactory : IDesignTimeDbContextFactory<NorthWindDomainLogsContext>
{
    public NorthWindDomainLogsContext CreateDbContext(string[] args)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new DBOptions
        {
            DomainLogsConnectionString = "Server=(localdb)\\mssqllocaldb;Database=NorthWindLogsDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        });

        return new NorthWindDomainLogsContext(options);
    }
}

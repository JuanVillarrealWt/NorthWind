namespace NorthWind.Membership.Backend.AspNetIdentity.DataContexts;

internal sealed class NorthWindMembershipContextFactory : IDesignTimeDbContextFactory<NorthWindMembershipContext>
{
    public NorthWindMembershipContext CreateDbContext(string[] args)
    {
        var options = Microsoft.Extensions.Options.Options.Create(new MembershipDBOptions
        {
            ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=NorthWindUsersDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        });
        return new NorthWindMembershipContext(options);
    }
}

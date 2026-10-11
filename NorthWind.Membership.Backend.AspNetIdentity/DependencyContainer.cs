namespace Microsoft.Extensions.DependencyInjection;

public static class MembershipIdentityDependencyContainer
{
    public static IServiceCollection AddMembershipIdentityServices(
        this IServiceCollection services,
        Action<MembershipDBOptions> configureMembershipDBOptions)
    {
        services.Configure(configureMembershipDBOptions);
        services.AddDbContext<NorthWindMembershipContext>();
        services.AddHostedService<MembershipDatabaseMigrationHostedService>();
        services.AddIdentityCore<NorthWindUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
        }).AddEntityFrameworkStores<NorthWindMembershipContext>();
        services.AddScoped<IMembershipService, MembershipService>();
        return services;
    }
}

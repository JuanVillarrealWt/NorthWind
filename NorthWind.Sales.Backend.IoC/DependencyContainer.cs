namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesServices(
        this IServiceCollection services,
        Action<DBOptions> configureDBOptions,
        Action<SmtpOptions> configureSmtpOptions,
        Action<MembershipDBOptions> configureMembershipDBOptions,
        Action<JwtOptions> configureJwtOptions)
    {
        services
            .AddUseCasesServices()
            .AddEventServices()
            .AddMailServices(configureSmtpOptions)
            .AddDomainLogsServices()
            .AddTransactionServices()
            .AddUserServices()
            .AddMembershipCoreServices(configureJwtOptions)
            .AddMembershipIdentityServices(configureMembershipDBOptions)
            .AddRepositories()
            .AddDataContexts(configureDBOptions)
            .AddPresenters()
            .AddValidationService()
            .AddSalesValidators()
            .AddValidationExceptionHandler()
            .AddUpdateExceptionHandler()
            .AddUnauthorizedAccessExceptionHandler()
            .AddUnhandledExceptionHandler();

        return services;
    }
}

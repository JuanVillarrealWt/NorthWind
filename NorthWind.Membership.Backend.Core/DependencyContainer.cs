namespace Microsoft.Extensions.DependencyInjection;

public static class MembershipCoreDependencyContainer
{
    public static IServiceCollection AddMembershipCoreServices(
        this IServiceCollection services,
        Action<JwtOptions> configureJwtOptions)
    {
        services.AddMembershipValidators();
        services.AddDefaultModelValidatorHub();
        services.AddScoped<NorthWind.Membership.Backend.Core.Interfaces.UserRegistration.IUserRegistrationInputPort,
            NorthWind.Membership.Backend.Core.UseCases.UserRegistration.UserRegistrationInteractor>();
        services.AddScoped<NorthWind.Membership.Backend.Core.Interfaces.UserRegistration.IUserRegistrationOutputPort,
            NorthWind.Membership.Backend.Core.Presenters.UserRegistration.UserRegistrationPresenter>();
        services.AddScoped<IUserLoginInputPort, UserLoginInteractor>();
        services.AddScoped<IUserLoginOutputPort, UserLoginPresenter>();
        services.AddSingleton<JwtService>();
        services.Configure(configureJwtOptions);
        return services;
    }
}

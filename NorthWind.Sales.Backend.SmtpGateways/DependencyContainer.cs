using NorthWind.Sales.Backend.SmtpGateways.Options;
using NorthWind.Sales.Backend.SmtpGateways.Services;
using NorthWind.Entities.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
    public static IServiceCollection AddMailServices(
        this IServiceCollection services,
        Action<SmtpOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddSingleton<IMailService, MailService>();
        return services;
    }
}

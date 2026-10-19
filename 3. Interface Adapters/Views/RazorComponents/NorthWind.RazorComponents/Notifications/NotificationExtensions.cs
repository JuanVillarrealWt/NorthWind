using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class NotificationExtensions
{
    public static IServiceCollection AddAppNotifications(this IServiceCollection services)
    {
        services.TryAddScoped<NorthWind.RazorComponents.Notifications.AppNotificationService>();
        return services;
    }
}

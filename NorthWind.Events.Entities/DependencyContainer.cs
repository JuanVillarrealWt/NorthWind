namespace Microsoft.Extensions.DependencyInjection;
public static class EventsDependencyContainer
{
 public static IServiceCollection AddEventServices(this IServiceCollection services)
 { services.AddScoped(typeof(NorthWind.Events.Entities.Interfaces.IDomainEventHub<>),typeof(NorthWind.Events.Entities.Services.DomainEventHub<>)); return services; }
}

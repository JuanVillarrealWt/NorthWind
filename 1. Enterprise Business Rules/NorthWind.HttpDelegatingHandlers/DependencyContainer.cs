namespace Microsoft.Extensions.DependencyInjection;
public static class HttpDelegatingHandlersDependencyContainer
{
 public static IServiceCollection AddExceptionDelegatingHandler(this IServiceCollection services)
 { services.TryAddTransient<NorthWind.HttpDelegatingHandlers.ExceptionDelegatingHandler>(); return services; }
}

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
 public static IServiceCollection AddValidationExceptionHandler(this IServiceCollection services) { services.AddExceptionHandler<NorthWind.Exceptions.Entities.ExceptionHandlers.ValidationExceptionHandler>(); return services; }
 public static IServiceCollection AddUpdateExceptionHandler(this IServiceCollection services) { services.AddExceptionHandler<NorthWind.Exceptions.Entities.ExceptionHandlers.UpdateExceptionHandler>(); return services; }
 public static IServiceCollection AddUnauthorizedAccessExceptionHandler(this IServiceCollection services) { services.AddExceptionHandler<NorthWind.Exceptions.Entities.ExceptionHandlers.UnauthorizedAccessExceptionHandler>(); return services; }
 public static IServiceCollection AddUnhandledExceptionHandler(this IServiceCollection services) { services.AddExceptionHandler<NorthWind.Exceptions.Entities.ExceptionHandlers.UnhandledExceptionHandler>(); return services; }
}

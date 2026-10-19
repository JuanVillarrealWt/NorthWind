namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
 public static IServiceCollection AddValidationService(this IServiceCollection services) => services.AddScoped(typeof(IValidationService<>),typeof(NorthWind.ValidationService.FluentValidation.FluentValidationService<>));
 public static IServiceCollection AddDefaultModelValidatorHub(this IServiceCollection services) => services.AddScoped(typeof(IModelValidatorHub<>),typeof(NorthWind.ValidationService.FluentValidation.ModelValidatorHub<>));
 public static IServiceCollection AddModelValidator<T,TValidator>(this IServiceCollection services) where TValidator:class,IModelValidator<T>
 { services.AddDefaultModelValidatorHub(); services.AddScoped<IModelValidator<T>,TValidator>(); return services; }
}

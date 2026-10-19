namespace Microsoft.Extensions.DependencyInjection;
public static class SalesValidatorsDependencyContainer
{
 public static IServiceCollection AddSalesValidators(this IServiceCollection services)
 {
  services.AddModelValidator<NorthWind.Sales.Entities.Dtos.CreateOrder.CreateOrderDto,NorthWind.Sales.Validators.Entities.CreateOrder.CreateOrderDtoValidator>();
  services.AddModelValidator<NorthWind.Sales.Entities.Dtos.CreateOrder.CreateOrderDetailDto,NorthWind.Sales.Validators.Entities.CreateOrder.CreateOrderDetailDtoValidator>();
  return services;
 }
}

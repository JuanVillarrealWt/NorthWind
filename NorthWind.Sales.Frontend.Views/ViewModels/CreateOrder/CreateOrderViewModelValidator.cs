namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;
internal sealed class CreateOrderViewModelValidator(IModelValidatorHub<CreateOrderDto> hub) : AbstractViewModelValidator<CreateOrderDto,CreateOrderViewModel>(hub,ValidationConstraint.AlwaysValidate);

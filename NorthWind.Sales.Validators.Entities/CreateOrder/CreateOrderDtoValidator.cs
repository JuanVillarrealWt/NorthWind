using System.Text.RegularExpressions;

namespace NorthWind.Sales.Validators.Entities.CreateOrder;

internal sealed class CreateOrderDtoValidator : AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(IValidationService<CreateOrderDto> service, IModelValidator<CreateOrderDetailDto> detailValidator) : base(service)
    {
        AddRuleFor(c => c.CustomerId).NotEmpty(CreateOrderMessages.CustomerIdRequired).Length(5, CreateOrderMessages.CustomerIdRequiredLength);
        AddRuleFor(c => c.ShipAddress).NotEmpty(CreateOrderMessages.ShipAddressRequired).MaximumLength(60, CreateOrderMessages.ShipAddressMaximumLength);
        AddRuleFor(c => c.ShipCity).NotEmpty(CreateOrderMessages.ShipCityRequired).MinimumLength(3, CreateOrderMessages.ShipCityMinimumLength).MaximumLength(15, CreateOrderMessages.ShipCityMaximumLength)
            .Must(city => string.IsNullOrEmpty(city) || Regex.IsMatch(city, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"), CreateOrderMessages.ShipCityMustBeLetters);
        AddRuleFor(c => c.ShipCountry).NotEmpty(CreateOrderMessages.ShipCountryRequired).MinimumLength(3, CreateOrderMessages.ShipCountryMinimumLength).MaximumLength(15, CreateOrderMessages.ShipCountryMaximumLength)
            .Must(country => string.IsNullOrEmpty(country) || Regex.IsMatch(country, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"), CreateOrderMessages.ShipCountryMustBeLetters);
        AddRuleFor(c => c.ShipPostalCode).MaximumLength(10, CreateOrderMessages.ShipPostalCodeMaximumLength);
        AddRuleFor(c => c.OrderDetails).NotNull(CreateOrderMessages.OrderDetailsRequired).NotEmpty(CreateOrderMessages.OrderDetailsNotEmpty);
        AddRuleForEach(c => c.OrderDetails).SetValidator(detailValidator);
    }
}

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderViewModel(ICreateOrderGateway gateway, IModelValidatorHub<CreateOrderViewModel> validator)
{
    public ModelValidator<CreateOrderViewModel> ModelValidatorComponentReference { get; set; } = default!;
    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;
    public string CustomerId { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipCountry { get; set; } = string.Empty;
    public string ShipPostalCode { get; set; } = string.Empty;
    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    public string InformationMessage { get; private set; } = string.Empty;

    public void AddNewOrderDetailItem() => OrderDetails.Add(new CreateOrderDetailViewModel());

    public async Task Send()
    {
        InformationMessage = string.Empty;
        try
        {
            var orderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            InformationMessage = $"[Transacción Completada con Éxito] {string.Format(System.Globalization.CultureInfo.CurrentCulture, CreateOrderMessages.CreatedOrderTemplate, orderId)}";
        }
        catch (HttpRequestException exception)
        {
            if (exception.Data["Errors"] is IEnumerable<ValidationError> errors)
            {
                ModelValidatorComponentReference.AddErrors(errors);
                InformationMessage = "[Transacción Fallida] Se encontraron errores de validación. La transacción no se realizó.";
            }
            else
            {
                InformationMessage = "[Transacción Fallida] Error de comunicación o concurrencia. La transacción fue revertida en el servidor.";
            }
        }
        catch (Exception)
        {
            InformationMessage = "[Transacción Fallida] Ocurrió un error al procesar la orden. La transacción fue cancelada y los cambios revertidos.";
        }
    }

    public static explicit operator CreateOrderDto(CreateOrderViewModel model) =>
        new(
            model.CustomerId,
            model.ShipAddress,
            model.ShipCity,
            model.ShipCountry,
            model.ShipPostalCode,
            model.OrderDetails.Select(detail => new CreateOrderDetailDto(
                detail.ProductId,
                detail.UnitPrice,
                detail.Quantity)));
}

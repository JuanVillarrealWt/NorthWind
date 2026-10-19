using NorthWind.RazorComponents.Formatting;
using NorthWind.RazorComponents.Notifications;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderViewModel(
    ICreateOrderGateway gateway,
    IModelValidatorHub<CreateOrderViewModel> validator,
    AppNotificationService notifications)
{
    public ModelValidator<CreateOrderViewModel> ModelValidatorComponentReference { get; set; } = default!;
    public IModelValidatorHub<CreateOrderViewModel> Validator => validator;
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    private string shipAddress = string.Empty;
    private string shipCity = string.Empty;
    private string shipCountry = string.Empty;
    private string shipPostalCode = string.Empty;

    public string ShipAddress
    {
        get => shipAddress;
        set => shipAddress = TextInputNormalizer.NormalizeInitialCapital(value);
    }

    public string ShipCity
    {
        get => shipCity;
        set => shipCity = TextInputNormalizer.NormalizeInitialCapital(value);
    }

    public string ShipCountry
    {
        get => shipCountry;
        set => shipCountry = TextInputNormalizer.NormalizeInitialCapital(value);
    }

    public string ShipPostalCode
    {
        get => shipPostalCode;
        set
        {
            var digits = TextInputNormalizer.KeepDigitsOnly(value);
            shipPostalCode = digits.Length <= 6 ? digits : digits[..6];
        }
    }
    public List<CreateOrderDetailViewModel> OrderDetails { get; set; } = [];
    public OrderReceipt? CompletedReceipt { get; private set; }
    public string CurrentSalesperson { get; set; } = string.Empty;

    public void AddNewOrderDetailItem() => OrderDetails.Add(new CreateOrderDetailViewModel());

    public async Task Send(string? salesperson = null)
    {
        CompletedReceipt = null;
        try
        {
            var orderId = await gateway.CreateOrderAsync((CreateOrderDto)this);
            var lines = OrderDetails
                .Where(detail => detail.ProductId > 0)
                .Select(detail => new OrderReceiptLine(
                    detail.ProductId,
                    string.IsNullOrWhiteSpace(detail.ProductName) ? $"Producto #{detail.ProductId}" : detail.ProductName,
                    detail.Quantity,
                    detail.UnitPrice))
                .ToList();

            var effectiveSeller = !string.IsNullOrWhiteSpace(salesperson)
                ? salesperson
                : !string.IsNullOrWhiteSpace(CurrentSalesperson)
                    ? CurrentSalesperson
                    : "Vendedor de Mostrador";

            CompletedReceipt = new OrderReceipt(
                orderId,
                CustomerId,
                string.IsNullOrWhiteSpace(CustomerName) ? CustomerId : CustomerName,
                DateTime.Now,
                ShipAddress,
                ShipCity,
                ShipCountry,
                ShipPostalCode,
                lines,
                SalespersonName: effectiveSeller);
        }
        catch (HttpRequestException exception)
        {
            if (exception.Data["Errors"] is IEnumerable<ValidationError> errors)
            {
                var errorMessages = errors.Select(error => error.Message).Where(message => !string.IsNullOrWhiteSpace(message)).Distinct().ToArray();
                var message = string.Join(" ", errorMessages);
                var hasStockError = message.Contains("existencias", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("stock", StringComparison.OrdinalIgnoreCase);
                var hasCustomerError = message.Contains("cliente", StringComparison.OrdinalIgnoreCase)
                    && message.Contains("no existe", StringComparison.OrdinalIgnoreCase);
                var hasDebtError = message.Contains("adeudo", StringComparison.OrdinalIgnoreCase)
                    || message.Contains("deuda", StringComparison.OrdinalIgnoreCase);
                notifications.ShowError(
                    "No se pudo crear la factura",
                    message.Length == 0 ? "Revisa los datos ingresados." : message,
                    hasStockError
                        ? "Reduce la cantidad solicitada o elige otro producto con existencias suficientes."
                        : hasDebtError
                            ? "Selecciona un cliente sin adeudos pendientes o registra el pago de la deuda antes de crear la factura."
                        : hasCustomerError
                            ? "Verifica el ID o registra al cliente antes de crear la factura."
                            : "Corrige los datos señalados y vuelve a intentar.");
            }
            else
            {
                notifications.ShowError(
                    "No se pudo crear la factura",
                    exception.StatusCode is null
                        ? "No fue posible comunicarse con el servidor."
                        : "El servidor rechazó la solicitud o detectó un conflicto al guardar.",
                    "Comprueba tu conexión y los datos de la orden; después vuelve a intentarlo.");
            }
        }
        catch (Exception)
        {
            notifications.ShowError(
                "No se pudo crear la factura",
                "Ocurrió un problema inesperado al procesar la orden.",
                "Verifica los datos e inténtalo de nuevo. Si el problema continúa, comunícate con soporte.");
        }
    }

    public void Reset()
    {
        CustomerId = string.Empty;
        CustomerName = string.Empty;
        ShipAddress = string.Empty;
        ShipCity = string.Empty;
        ShipCountry = string.Empty;
        ShipPostalCode = string.Empty;
        OrderDetails = [new CreateOrderDetailViewModel()];
        CompletedReceipt = null;
        ModelValidatorComponentReference?.ClearErrors();
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

public sealed record OrderReceipt(
    int OrderId,
    string CustomerId,
    string CustomerName,
    DateTime CreatedAt,
    string ShipAddress,
    string ShipCity,
    string ShipCountry,
    string ShipPostalCode,
    IReadOnlyList<OrderReceiptLine> Lines,
    string SalespersonName = "Vendedor de Mostrador",
    string CompanyName = "Ventas NorthWind",
    string CompanyTaxId = "",
    string CompanyAddress = "",
    string CompanyCity = "",
    string CompanyPhone = "",
    string CompanyEmail = "",
    string CompanyWebsite = "")
{
    public decimal Subtotal => Lines.Sum(line => line.Subtotal);
    public decimal Tax => Subtotal * 0.15m;
    public decimal Total => Subtotal + Tax;
}

public sealed record OrderReceiptLine(int ProductId, string ProductName, short Quantity, decimal UnitPrice)
{
    public decimal Subtotal => UnitPrice * Quantity;
}

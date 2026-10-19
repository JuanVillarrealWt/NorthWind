using NorthWind.RazorComponents.Formatting;

namespace NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;

public class CreateOrderDetailViewModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string UnitPriceInput => UnitPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
    private short quantity = 1;
    private string quantityInput = "1";

    public string ProductIdInput
    {
        get => ProductId > 0 ? ProductId.ToString(System.Globalization.CultureInfo.InvariantCulture) : string.Empty;
        set
        {
            var digits = TextInputNormalizer.KeepDigitsOnly(value);
            if (digits.Length > 5) digits = digits[..5];
            var productId = int.TryParse(digits, out var parsedProductId) ? parsedProductId : 0;
            if (ProductId != productId) ProductName = string.Empty;
            ProductId = productId;
        }
    }

    public short Quantity
    {
        get => quantity;
        set
        {
            quantity = (short)Math.Clamp((int)value, 1, short.MaxValue);
            quantityInput = quantity.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public void IncrementQuantity()
    {
        var nextQuantity = Math.Max((int)quantity, 1);
        if (nextQuantity < short.MaxValue) nextQuantity++;
        Quantity = (short)nextQuantity;
    }

    public string QuantityInput
    {
        get => quantityInput;
        set
        {
            var digits = TextInputNormalizer.KeepDigitsOnly(value);
            if (digits.Length == 0)
            {
                quantity = 0;
                quantityInput = string.Empty;
                return;
            }

            if (!int.TryParse(digits, out var parsedQuantity) || parsedQuantity > short.MaxValue)
            {
                quantity = short.MaxValue;
                quantityInput = short.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                return;
            }

            quantity = (short)Math.Max(1, parsedQuantity);
            quantityInput = quantity.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}

using System.Globalization;
using System.Resources;

namespace NorthWind.Sales.Frontend.Views.Resources;

public static class CreateOrderMessages
{
    private static readonly ResourceManager ResourceManager = new(
        "NorthWind.Sales.Frontend.Views.Resources.CreateOrderMessages",
        typeof(CreateOrderMessages).Assembly);

    private static string Get(string name) => ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? name;

    public static string CustomerIdLabel => Get(nameof(CustomerIdLabel));
    public static string ShipAddressLabel => Get(nameof(ShipAddressLabel));
    public static string ShipCityLabel => Get(nameof(ShipCityLabel));
    public static string ShipCountryLabel => Get(nameof(ShipCountryLabel));
    public static string ShipPostalCodeLabel => Get(nameof(ShipPostalCodeLabel));
    public static string ProductIdLabel => Get(nameof(ProductIdLabel));
    public static string UnitPriceLabel => Get(nameof(UnitPriceLabel));
    public static string QuantityLabel => Get(nameof(QuantityLabel));
    public static string AddProductButtonText => Get(nameof(AddProductButtonText));
    public static string SendButtonText => Get(nameof(SendButtonText));
    public static string CreatedOrderTemplate => Get(nameof(CreatedOrderTemplate));
    public static string CreateOrderMenuItemTitle => Get(nameof(CreateOrderMenuItemTitle));
    public static string CreateOrderPageTitle => Get(nameof(CreateOrderPageTitle));
}

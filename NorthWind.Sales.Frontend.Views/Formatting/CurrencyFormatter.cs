using System.Globalization;

namespace NorthWind.Sales.Frontend.Views.Formatting;

public static class CurrencyFormatter
{
    private const string UsdFormat = "\"$\"#,##0.00;-\"$\"#,##0.00";

    public static string Format(decimal amount) => amount.ToString(UsdFormat, CultureInfo.InvariantCulture);
}

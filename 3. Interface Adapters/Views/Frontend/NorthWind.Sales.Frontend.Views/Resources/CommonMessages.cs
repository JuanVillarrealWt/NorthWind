using System.Globalization;
using System.Resources;

namespace NorthWind.Sales.Frontend.Views.Resources;

public static class CommonMessages
{
    private static readonly ResourceManager ResourceManager = new(
        "NorthWind.Sales.Frontend.Views.Resources.CommonMessages",
        typeof(CommonMessages).Assembly);

    private static string Get(string name) => ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? name;

    public static string OK => Get(nameof(OK));
    public static string NavBarBrandTitle => Get(nameof(NavBarBrandTitle));
    public static string HomeMenuItemTitle => Get(nameof(HomeMenuItemTitle));
    public static string AboutTitle => Get(nameof(AboutTitle));
}

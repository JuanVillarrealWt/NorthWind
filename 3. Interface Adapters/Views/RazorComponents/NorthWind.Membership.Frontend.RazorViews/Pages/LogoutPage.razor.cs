namespace NorthWind.Membership.Frontend.RazorViews.Pages;

public partial class LogoutPage
{
    [Inject]
    public JWTAuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    private async Task CloseSession()
    {
        await AuthenticationStateProvider.LogoutAsync();
        NavigationManager.NavigateTo("/");
    }
}

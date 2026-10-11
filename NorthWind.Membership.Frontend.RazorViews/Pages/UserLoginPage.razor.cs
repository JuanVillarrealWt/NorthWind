namespace NorthWind.Membership.Frontend.RazorViews.Pages;

public partial class UserLoginPage
{
    private const string RouteTemplate = "/user/login";

    [Inject]
    public UserLoginViewModel ViewModel { get; set; } = default!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    private ErrorBoundary? ErrorBoundaryRef { get; set; }

    protected override void OnInitialized() => ViewModel.OnLogin += OnLogin;

    private void Recover() => ErrorBoundaryRef?.Recover();

    private void OnLogin()
    {
        if (NavigationManager.Uri.EndsWith(RouteTemplate, StringComparison.OrdinalIgnoreCase))
            NavigationManager.NavigateTo(string.Empty);
        else
            NavigationManager.NavigateTo(NavigationManager.Uri);
    }
}

namespace NorthWind.Membership.Frontend.RazorViews.Pages;

public partial class UserRegistrationPage
{
    [Inject]
    public UserRegistrationViewModel ViewModel { get; set; } = default!;
    private ErrorBoundary? ErrorBoundaryRef { get; set; }
    private void Recover() => ErrorBoundaryRef?.Recover();
}

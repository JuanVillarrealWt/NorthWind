namespace NorthWind.Membership.Frontend.RazorViews.Components;

public partial class UserRegistrationComponent
{
    [Parameter, EditorRequired]
    public UserRegistrationViewModel ViewModel { get; set; } = default!;
}

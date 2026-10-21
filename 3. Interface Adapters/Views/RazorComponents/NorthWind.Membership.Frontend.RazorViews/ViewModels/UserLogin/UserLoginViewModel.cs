using NorthWind.RazorComponents.Notifications;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserLogin;

public sealed class UserLoginViewModel(
    MembershipGateway gateway,
    IModelValidatorHub<UserLoginViewModel> validator,
    JWTAuthenticationStateProvider authenticationStateProvider,
    AppNotificationService notifications)
{
    public IModelValidatorHub<UserLoginViewModel> Validator => validator;
    public ModelValidator<UserLoginViewModel> ModelValidatorComponentReference { get; set; } = default!;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public event Action? OnLogin;

    public async Task Login()
    {
        try
        {
            var tokens = await gateway.LoginAsync((UserCredentialsDto)this);
            await authenticationStateProvider.LoginAsync(tokens);
            OnLogin?.Invoke();
        }
        catch (HttpRequestException exception)
        {
            if (exception.Data["Errors"] is IEnumerable<ValidationError> errors)
            {
                var message = string.Join(" ", errors.Select(error => error.Message).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct());
                notifications.ShowError(
                    "No se pudo iniciar sesión",
                    string.IsNullOrWhiteSpace(message) ? "Las credenciales no son válidas." : message,
                    "Comprueba el correo y la contraseña e inténtalo de nuevo.");
            }
            else
                throw;
        }
    }

    public static explicit operator UserCredentialsDto(UserLoginViewModel model) =>
        new(model.Email.Trim(), model.Password);
}

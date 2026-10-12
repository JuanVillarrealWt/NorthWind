using NorthWind.RazorComponents.Formatting;
using NorthWind.RazorComponents.Notifications;

namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserRegistration;

public sealed class UserRegistrationViewModel(
    MembershipGateway gateway,
    IModelValidatorHub<UserRegistrationViewModel> validator,
    AppNotificationService notifications)
{
    public IModelValidatorHub<UserRegistrationViewModel> Validator => validator;
    public ModelValidator<UserRegistrationViewModel> ModelValidatorComponentReference { get; set; } = default!;
    public string InformationMessage { get; private set; } = string.Empty;
    private string firstName = string.Empty;
    private string lastName = string.Empty;

    public string FirstName
    {
        get => firstName;
        set => firstName = TextInputNormalizer.NormalizeInitialCapital(value);
    }

    public string LastName
    {
        get => lastName;
        set => lastName = TextInputNormalizer.NormalizeInitialCapital(value);
    }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirmation { get; set; } = string.Empty;

    public async Task Register()
    {
        try
        {
            InformationMessage = string.Empty;
            await gateway.RegisterAsync((UserRegistrationDto)this);
            InformationMessage = UserRegistrationMessages.RegisteredUserMessage;
        }
        catch (HttpRequestException exception)
        {
            if (exception.Data["Errors"] is IEnumerable<ValidationError> errors)
            {
                var message = string.Join(" ", errors.Select(error => error.Message).Where(value => !string.IsNullOrWhiteSpace(value)).Distinct());
                notifications.ShowError(
                    "No se pudo crear la cuenta",
                    string.IsNullOrWhiteSpace(message) ? "Algunos datos no son válidos." : message,
                    "Revisa los datos señalados, corrige lo necesario y vuelve a intentarlo.");
            }
            else
                throw;
        }
    }

    public static explicit operator UserRegistrationDto(UserRegistrationViewModel model) =>
        new(model.Email, model.Password, model.FirstName, model.LastName);
}

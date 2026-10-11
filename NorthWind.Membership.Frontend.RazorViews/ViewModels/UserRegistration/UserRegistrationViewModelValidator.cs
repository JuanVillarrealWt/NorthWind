namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserRegistration;

internal sealed class UserRegistrationViewModelValidator : AbstractModelValidator<UserRegistrationViewModel>
{
    public UserRegistrationViewModelValidator(IValidationService<UserRegistrationViewModel> validationService) : base(validationService, ValidationConstraint.AlwaysValidate)
    {
        AddRuleFor(model => model.PasswordConfirmation)
            .Equal(model => model.Password, UserRegistrationMessages.ConfirmPasswordErrorMessage);
    }
}

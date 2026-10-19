namespace NorthWind.Membership.Entities.Validators.UserRegistration;

internal sealed class UserRegistrationDtoValidator : AbstractModelValidator<UserRegistrationDto>
{
    public UserRegistrationDtoValidator(IValidationService<UserRegistrationDto> validationService) : base(validationService)
    {
        AddRuleFor(user => user.FirstName).NotEmpty(UserRegistrationMessages.RequiredFirstNameErrorMessage);
        AddRuleFor(user => user.LastName).NotEmpty(UserRegistrationMessages.RequiredLastNameErrorMessage);
        AddRuleFor(user => user.Email)
            .NotEmpty(UserRegistrationMessages.RequiredEmailErrorMessage)
            .EmailAddress(UserRegistrationMessages.InvalidEmailErrorMessage);
        AddRuleFor(user => user.Password)
            .StopOnFirstFailure()
            .NotEmpty(UserRegistrationMessages.RequiredPasswordErrorMessage)
            .MinimumLength(6, UserRegistrationMessages.PasswordTooShortErrorMessage)
            .Must(password => password.Any(char.IsLower), UserRegistrationMessages.PasswordRequiresLowerErrorMessage)
            .Must(password => password.Any(char.IsUpper), UserRegistrationMessages.PasswordRequiresUpperErrorMessage)
            .Must(password => password.Any(char.IsDigit), UserRegistrationMessages.PasswordRequiresDigitErrorMessage)
            .Must(password => password.Any(character => !char.IsLetterOrDigit(character)), UserRegistrationMessages.PasswordRequiresNonAlphanumericErrorMessage);
    }
}

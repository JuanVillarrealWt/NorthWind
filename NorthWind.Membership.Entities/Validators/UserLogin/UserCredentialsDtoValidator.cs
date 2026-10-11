namespace NorthWind.Membership.Entities.Validators.UserLogin;

internal sealed class UserCredentialsDtoValidator : AbstractModelValidator<UserCredentialsDto>
{
    public UserCredentialsDtoValidator(IValidationService<UserCredentialsDto> validationService) : base(validationService)
    {
        AddRuleFor(user => user.Email)
            .NotEmpty(UserLoginMessages.RequiredEmailErrorMessage)
            .EmailAddress(UserLoginMessages.InvalidEmailErrorMessage);
        AddRuleFor(user => user.Password).NotEmpty(UserLoginMessages.RequiredPasswordErrorMessage);
    }
}

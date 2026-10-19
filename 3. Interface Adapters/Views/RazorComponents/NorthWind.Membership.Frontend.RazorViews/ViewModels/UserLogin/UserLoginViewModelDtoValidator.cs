namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserLogin;

internal sealed class UserLoginViewModelDtoValidator(
    IModelValidatorHub<UserCredentialsDto> validator)
    : AbstractViewModelValidator<UserCredentialsDto, UserLoginViewModel>(
        validator,
        ValidationConstraint.AlwaysValidate)
{
}

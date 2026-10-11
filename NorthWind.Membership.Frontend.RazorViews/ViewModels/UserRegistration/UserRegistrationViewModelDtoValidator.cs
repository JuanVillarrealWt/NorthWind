namespace NorthWind.Membership.Frontend.RazorViews.ViewModels.UserRegistration;

internal sealed class UserRegistrationViewModelDtoValidator(IModelValidatorHub<UserRegistrationDto> validator)
    : AbstractViewModelValidator<UserRegistrationDto, UserRegistrationViewModel>(validator, ValidationConstraint.AlwaysValidate);

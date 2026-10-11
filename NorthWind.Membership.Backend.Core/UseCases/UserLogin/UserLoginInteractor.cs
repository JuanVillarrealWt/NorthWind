namespace NorthWind.Membership.Backend.Core.UseCases.UserLogin;

internal sealed class UserLoginInteractor(
    IMembershipService membershipService,
    IUserLoginOutputPort presenter,
    IModelValidatorHub<UserCredentialsDto> validationService) : IUserLoginInputPort
{
    public async Task Handle(UserCredentialsDto userData)
    {
        Result<UserDto, IEnumerable<ValidationError>> result;
        if (!await validationService.Validate(userData))
        {
            result = new Result<UserDto, IEnumerable<ValidationError>>(validationService.Errors);
        }
        else
        {
            var user = await membershipService.GetUserByCredentials(userData);
            result = user is null
                ? new Result<UserDto, IEnumerable<ValidationError>>(
                    [new ValidationError(nameof(userData.Password), CoreUserLoginMessages.InvalidUserCredentialsErrorMessage)])
                : new Result<UserDto, IEnumerable<ValidationError>>(user);
        }

        await presenter.Handle(result);
    }
}

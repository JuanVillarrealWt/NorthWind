namespace NorthWind.Membership.Backend.Core.Presenters.UserLogin;

internal sealed class UserLoginPresenter(JwtService service) : IUserLoginOutputPort
{
    public IResult Result { get; private set; } = Results.Ok();

    public Task Handle(Result<UserDto, IEnumerable<ValidationError>> userLoginResult)
    {
        userLoginResult.HandleResult(
            userDto => Result = Results.Ok(new TokensDto(service.GetToken(userDto))),
            errors => Result = Results.Problem(errors.ToProblemDetails(
                CoreUserLoginMessages.UserLoginErrorTitle,
                CoreUserLoginMessages.UserLoginErrorDetail,
                nameof(UserLoginPresenter))));
        return Task.CompletedTask;
    }
}

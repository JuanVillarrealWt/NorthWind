namespace NorthWind.Membership.Backend.Core.Presenters.UserRegistration;

internal sealed class UserRegistrationPresenter : IUserRegistrationOutputPort
{
    public IResult Result { get; private set; } = Results.Ok();

    public Task Handle(Result<IEnumerable<ValidationError>> registrationResult)
    {
        registrationResult.HandleResult(
            errors => Result = Results.Problem(errors.ToProblemDetails(
                MembershipCoreMessages.UserRegistrationErrorTitle,
                MembershipCoreMessages.UserRegistrationErrorDetail,
                nameof(UserRegistrationPresenter))),
            () => Result = Results.Ok());
        return Task.CompletedTask;
    }
}

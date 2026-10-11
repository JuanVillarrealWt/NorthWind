namespace NorthWind.Membership.Backend.AspNetIdentity.Services;

internal sealed class MembershipService(UserManager<NorthWindUser> manager) : IMembershipService
{
    public async Task<Result<IEnumerable<ValidationError>>> Register(UserRegistrationDto userData)
    {
        var user = new NorthWindUser
        {
            UserName = userData.Email,
            Email = userData.Email,
            FirstName = userData.FirstName,
            LastName = userData.LastName
        };
        var createResult = await manager.CreateAsync(user, userData.Password);
        return createResult.Succeeded
            ? new Result<IEnumerable<ValidationError>>()
            : new Result<IEnumerable<ValidationError>>(createResult.Errors.ToValidationErrors());
    }

    public async Task<UserDto?> GetUserByCredentials(UserCredentialsDto userData)
    {
        var user = await manager.FindByNameAsync(userData.Email);
        if (user is null || !await manager.CheckPasswordAsync(user, userData.Password))
            return null;

        return new UserDto(user.UserName ?? userData.Email, user.FirstName, user.LastName);
    }
}

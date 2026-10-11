namespace NorthWind.Membership.Entities.Dtos.UserLogin;

public sealed class UserCredentialsDto(string email, string password)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}

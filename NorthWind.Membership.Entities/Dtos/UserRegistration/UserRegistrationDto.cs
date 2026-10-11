namespace NorthWind.Membership.Entities.Dtos.UserRegistration;

public sealed class UserRegistrationDto(string email, string password, string firstName, string lastName)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}

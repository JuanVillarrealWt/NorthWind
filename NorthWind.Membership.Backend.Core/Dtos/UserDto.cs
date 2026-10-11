namespace NorthWind.Membership.Backend.Core.Dtos;

public sealed class UserDto(string email, string firstName, string lastName)
{
    public string Email { get; } = email;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}

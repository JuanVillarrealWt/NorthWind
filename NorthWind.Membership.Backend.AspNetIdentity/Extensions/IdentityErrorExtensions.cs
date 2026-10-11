namespace NorthWind.Membership.Backend.AspNetIdentity.Extensions;

internal static class IdentityErrorExtensions
{
    public static IEnumerable<ValidationError> ToValidationErrors(this IEnumerable<IdentityError> errors)
    {
        var result = new List<ValidationError>();
        foreach (var error in errors)
        {
            if (error.Code == nameof(IdentityErrorDescriber.DuplicateUserName))
                result.Add(new ValidationError(nameof(UserRegistrationDto.Email), Messages.DuplicateUserNameErrorMessage));
            else
                result.Add(new ValidationError(error.Code, error.Description));
        }
        return result;
    }
}

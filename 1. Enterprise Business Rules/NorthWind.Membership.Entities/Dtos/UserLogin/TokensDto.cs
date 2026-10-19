namespace NorthWind.Membership.Entities.Dtos.UserLogin;

public sealed class TokensDto(string accessToken)
{
    public string AccessToken { get; } = accessToken;
}

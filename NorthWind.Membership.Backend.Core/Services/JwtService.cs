namespace NorthWind.Membership.Backend.Core.Services;

internal sealed class JwtService(IOptions<JwtOptions> options)
{
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(options.Value.SecurityKey);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static List<Claim> GetClaims(UserDto userDto) =>
    [
        new Claim(ClaimTypes.Name, userDto.Email),
        new Claim("FullName", $"{userDto.FirstName} {userDto.LastName}")
    ];

    private SecurityTokenDescriptor GetTokenDescriptor(SigningCredentials signingCredentials, List<Claim> claims) => new()
    {
        Subject = new ClaimsIdentity(claims),
        Issuer = options.Value.ValidIssuer,
        Audience = options.Value.ValidAudience,
        Expires = DateTime.UtcNow.AddMinutes(options.Value.ExpireInMinutes),
        SigningCredentials = signingCredentials
    };

    public string GetToken(UserDto userData)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(userData);
        var tokenDescriptor = GetTokenDescriptor(signingCredentials, claims);
        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }
}

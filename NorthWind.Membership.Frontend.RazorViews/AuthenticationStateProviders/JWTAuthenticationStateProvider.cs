namespace NorthWind.Membership.Frontend.RazorViews.AuthenticationStateProviders;

public sealed class JWTAuthenticationStateProvider(ITokenStorage storage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();
        var tokens = await storage.GetTokensAsync();
        if (tokens is not null)
        {
            var token = new JsonWebTokenHandler().ReadJsonWebToken(tokens.AccessToken);
            identity = new ClaimsIdentity(token.Claims, nameof(JWTAuthenticationStateProvider));
        }

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async ValueTask LoginAsync(TokensDto tokens)
    {
        await storage.StoreTokensAsync(tokens);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async ValueTask LogoutAsync()
    {
        await storage.RemoveTokensAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

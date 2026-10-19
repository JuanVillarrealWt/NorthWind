namespace NorthWind.Membership.Frontend.RazorViews.WebApiGateways;

public sealed class MembershipGateway(HttpClient client)
{
    public async Task RegisterAsync(UserRegistrationDto userData)
    {
        using var response = await client.PostAsJsonAsync(Endpoints.Register, userData);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TokensDto> LoginAsync(UserCredentialsDto userCredentials)
    {
        using var response = await client.PostAsJsonAsync(Endpoints.Login, userCredentials);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TokensDto>()
            ?? throw new InvalidOperationException("La API devolvió una respuesta de inicio de sesión vacía.");
    }
}

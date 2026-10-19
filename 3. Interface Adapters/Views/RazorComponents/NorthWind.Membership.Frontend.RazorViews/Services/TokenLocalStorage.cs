namespace NorthWind.Membership.Frontend.RazorViews.Services;

internal sealed class TokenLocalStorage(IJSRuntime jsRuntime) : ITokenStorage
{
    private const string StorageKey = "tokens";
    private const string StoreIdentifier = "localStorage.setItem";
    private const string GetIdentifier = "localStorage.getItem";
    private const string RemoveIdentifier = "localStorage.removeItem";

    public async Task StoreTokensAsync(TokensDto tokens)
    {
        var value = JsonSerializer.Serialize(tokens);
        await jsRuntime.InvokeVoidAsync(StoreIdentifier, StorageKey, value);
    }

    public async Task<TokensDto?> GetTokensAsync()
    {
        var value = await jsRuntime.InvokeAsync<string?>(GetIdentifier, StorageKey);
        return string.IsNullOrEmpty(value) ? null : JsonSerializer.Deserialize<TokensDto>(value);
    }

    public Task RemoveTokensAsync() => jsRuntime.InvokeVoidAsync(RemoveIdentifier, StorageKey).AsTask();
}

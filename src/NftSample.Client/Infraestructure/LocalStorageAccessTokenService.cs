using Microsoft.JSInterop;

namespace NftSample.Client.Infraestructure;

public class LocalStorageAccessTokenService(IJSRuntime jSRuntime) : IAccessTokenService
{
    public const string JWTTokenName = "jwt_token";

    public async Task<string> GetAsync()
    {
        return await jSRuntime.InvokeAsync<string>("localStorage.getItem", JWTTokenName);
    }

    public async Task SetAsync(string tokenValue)
    {
        await jSRuntime.InvokeVoidAsync("localStorage.setItem", JWTTokenName, tokenValue);
    }

    public async Task RemoveAsync()
    {
        await jSRuntime.InvokeAsync<string>("localStorage.removeItem", JWTTokenName);
    }
}
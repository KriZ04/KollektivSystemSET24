using Microsoft.JSInterop;

namespace KollektivSystem.Web.Services;

public sealed class TokenStore(IJSRuntime js) : ITokenStore
{
    public Task SetAsync(string token) => js.InvokeVoidAsync("localStorage.setItem", "api_jwt", token).AsTask();
    public async Task<string?> GetAsync() => await js.InvokeAsync<string?>("localStorage.getItem", "api_jwt");
    public Task ClearAsync() => js.InvokeVoidAsync("localStorage.removeItem", "api_jwt").AsTask();
}

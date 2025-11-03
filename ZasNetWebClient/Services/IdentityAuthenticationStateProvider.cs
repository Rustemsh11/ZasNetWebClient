using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ZasNetWebClient.Services;

public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorageService;

    public IdentityAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await this.localStorageService.GetItemAsync<string>("token");
        var expiredDateTIme = await this.localStorageService.GetItemAsync<DateTime>("expiredDate");
        if (string.IsNullOrEmpty(token) || expiredDateTIme > DateTime.Now)
        {
            return Empty();
        }
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var nameClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        var roleClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role");
        if (!string.IsNullOrEmpty(token))
        {
            var authUser = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>()
                {
                    nameClaim,
                    roleClaim,
                }, "jwt"));

            return new AuthenticationState(authUser);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);

        var nameClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
        var roleClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role");
            var authUser = new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>()
                {
                    nameClaim,
                    roleClaim,
                }, "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public async Task MarkLogouted()
    {
        await localStorageService.RemoveItemAsync("token");
        NotifyAuthenticationStateChanged(Task.FromResult(Empty()));
    }
    private static AuthenticationState Empty()
           => new(new ClaimsPrincipal(new ClaimsIdentity()));
}

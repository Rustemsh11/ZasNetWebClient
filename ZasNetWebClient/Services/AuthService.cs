using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using ZasNetWebClient.Models;

namespace ZasNetWebClient.Services;

public class AuthService
{
    private ILocalStorageService localStorageService;
    private IdentityAuthenticationStateProvider identityAuthenticationStateProvider;

    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        this.localStorageService = localStorageService;
        this._httpClient = httpClient;
        this.identityAuthenticationStateProvider = (IdentityAuthenticationStateProvider)authenticationStateProvider;
    }

    public async Task<bool> Login(string login, string password)
    {
        try
        {
            var loginRequest = new LoginRequest { Login = login, Password = password };

            // Call the server's authentication endpoint
            var response = await _httpClient.PostAsJsonAsync("/api/v1/Auth/GetAuthToken", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                string? token = null;

                // Try JSON response first
                try
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (!string.IsNullOrEmpty(loginResponse.Token))
                    {
                        token = loginResponse.Token;
                    }
                }
                catch
                {
                    // Fallback to raw string body
                }

                if (string.IsNullOrEmpty(token))
                {
                    token = await response.Content.ReadAsStringAsync();
                    token = token?.Trim('"');
                }

                if (!string.IsNullOrEmpty(token))
                {
                    await localStorageService.SetItemAsStringAsync("token", token);
                    this.identityAuthenticationStateProvider.MarkUserAsAuthenticated(token);

                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return false;
        }
    }

    public void Logout()
    {

        //OnAuthenticationStateChanged?.Invoke();
    }
}


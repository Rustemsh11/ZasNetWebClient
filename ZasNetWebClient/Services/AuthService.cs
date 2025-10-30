using ZasNetWebClient.Models;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.JSInterop;

namespace ZasNetWebClient.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private User? _currentUser;
    private string? _token;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;

        // Try to restore session on service creation
        _ = RestoreSessionAsync();
    }

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _currentUser != null;
    public bool IsManager => _currentUser?.RoleName == "Manager";

    public event Action? OnAuthenticationStateChanged;

    public async Task<bool> LoginAsync(string login, string password)
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
                    if (loginResponse?.Success == true && !string.IsNullOrEmpty(loginResponse.Token))
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
                    _token = token;
                    // Persist token in localStorage for future sessions
                    try { await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _token); } catch {}
                    
                    // Parse user information from JWT token claims
                    _currentUser = ParseUserFromToken(token, login);
                    
                    OnAuthenticationStateChanged?.Invoke();
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

    private async Task RestoreSessionAsync()
    {
        try
        {
            var storedToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrWhiteSpace(storedToken))
            {
                _token = storedToken.Trim('"');
                _currentUser = ParseUserFromToken(_token, _currentUser?.Login ?? string.Empty);
                OnAuthenticationStateChanged?.Invoke();
            }
        }
        catch
        {
            // Ignore restore errors
        }
    }

    private User ParseUserFromToken(string token, string login)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            
            var nameClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            var roleClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role");
            
            return new User
            {
                Login = nameClaim?.Value ?? login,
                RoleName = roleClaim?.Value ?? "User"
            };
        }
        catch
        {
            // Fallback if token parsing fails
            return new User
            {
                Login = login,
                RoleName = "User"
            };
        }
    }

    public void Logout()
    {
        _currentUser = null;
        _token = null;
        // Best-effort clear of stored token
        try { _ = _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken"); } catch {}
        OnAuthenticationStateChanged?.Invoke();
    }

    public string? GetToken()
    {
        return _token;
    }
}


using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ZasNetWebClient.Models;

namespace ZasNetWebClient.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;
    private readonly ILocalStorageService _localStorageService;

    public ApiService(HttpClient httpClient, AuthService authService, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _authService = authService;
        _localStorageService = localStorageService;
    }
    
    public async Task<List<Order>> GetAllOrders()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var orders = await _httpClient.GetFromJsonAsync<List<Order>>("api/v1/order/getorders");
            return orders ?? new List<Order>();
        }
        catch
        {
            return new List<Order>();
        }
    }
    
    public async Task<CreateOrderParameters> GetCreateOrderParameters()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var createOrderParameters = await _httpClient.GetFromJsonAsync<CreateOrderParameters>("api/v1/order/GetCreateOrderParametSers");

            return createOrderParameters ?? new CreateOrderParameters();
        }
        catch(Exception ex)
        {
            return new CreateOrderParameters();
        }
    }
}

using ZasNetWebClient.Models;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace ZasNetWebClient.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public ApiService(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
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
        catch(Exception ex)
        {
            return new List<Order>();
        }
    }
    
    public async Task<OrderDto> GetOrder(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var order = await _httpClient.GetFromJsonAsync<OrderDto>($"api/v1/Order/GetOrder?orderId={id}");
            return order ?? new OrderDto();
        }
        catch(Exception ex)
        {
            return new OrderDto();
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

    public async Task<bool> CreateOrder(OrderDto orderDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            orderDto.UserId = await _localStorageService.GetItemAsync<int>("userId");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var response = await _httpClient.PostAsJsonAsync("api/v1/order/CreateOrder", orderDto);
            
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
    
    public async Task<bool> SaveOrder(int orderId, OrderDto orderDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            orderDto.UserId = await _localStorageService.GetItemAsync<int>("userId");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync($"api/v1/order/saveOrder?orderId={orderId}", orderDto);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> LockOrder(int orderId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsync($"api/v1/order/lock?orderId={orderId}", new StringContent(string.Empty));
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task UnlockOrder(int orderId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            await _httpClient.PostAsync($"api/v1/order/Unlock?orderId={orderId}", new StringContent(string.Empty));
        }
        catch
        {
            // ignore unlock errors
        }
    }
}

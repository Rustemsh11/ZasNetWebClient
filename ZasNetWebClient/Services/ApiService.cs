using ZasNetWebClient.Models;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Forms;

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

            var createOrderDto = new CreateOrderCommand() { OrderDto = orderDto };
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var response = await _httpClient.PostAsJsonAsync("api/v1/order/CreateOrder", createOrderDto);
            
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            return false;
        }
    }
    
    public async Task<bool> SaveOrder(OrderDto orderDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            var saveOrderDto = new SaveOrderCommand() { OrderDto = orderDto };
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync($"api/v1/order/SaveOrder", saveOrderDto);

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

    public async Task<List<CarDto>> GetAllCars()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var cars = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/v1/car/GetAllCars");
            return cars ?? new List<CarDto>();
        }
        catch(Exception ex)
        {
            return new List<CarDto>();
        }
    }

    public async Task<bool> AddDocument(IEnumerable<IBrowserFile> files, int orderId, DocumentType documentType, string? description = null, int? uploadedUserId = null)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            using var content = new MultipartFormDataContent();

            // Add files (collection)
            var fileList = files?.ToList() ?? new List<IBrowserFile>();
            foreach (var file in fileList)
            {
                if (file != null && file.Size > 0)
                {
                    var fileContent = new StreamContent(file.OpenReadStream(10 * 1024 * 1024)); // 10MB max
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "Files", file.Name);
                }
            }

            // Add other fields
            content.Add(new StringContent(orderId.ToString()), "OrderId");
            content.Add(new StringContent(((int)documentType).ToString()), "DocumentType");

            if (!string.IsNullOrEmpty(description))
            {
                content.Add(new StringContent(description), "Description");
            }

            if (uploadedUserId.HasValue)
            {
                content.Add(new StringContent(uploadedUserId.Value.ToString()), "UploadedUserId");
            }

            var response = await _httpClient.PostAsync("api/v1/document/adddocument", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}

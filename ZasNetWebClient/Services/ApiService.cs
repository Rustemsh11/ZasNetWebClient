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
    private readonly NotificationService _notificationService;

    public ApiService(HttpClient httpClient, ILocalStorageService localStorageService, NotificationService notificationService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _notificationService = notificationService;
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
            _notificationService.ShowError($"Ошибка при загрузке заявок: {ex.Message}");
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
            _notificationService.ShowError($"Ошибка при загрузке заявки: {ex.Message}");
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
            _notificationService.ShowError($"Ошибка при загрузке параметров заявки: {ex.Message}");
            return new CreateOrderParameters();
        }
    }
    
    public async Task<List<EmployeeDto>> GetDispetchers()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var dispetchers = await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("api/v1/employee/GetDispetchers");

            return dispetchers ?? new List<EmployeeDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке диспетчеров: {ex.Message}");
            return new List<EmployeeDto>();
        }
    }
    
    public async Task<List<EmployeeDto>> GetDrivers()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var drivers = await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("api/v1/employee/GetDrivers");

            return drivers ?? new List<EmployeeDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке водителей: {ex.Message}");
            return new List<EmployeeDto>();
        }
    }
    
    public async Task<List<ServiceDto>> GetServices()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var services = await _httpClient.GetFromJsonAsync<List<ServiceDto>>("api/v1/service/GetServices");

            return services ?? new List<ServiceDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке услуг: {ex.Message}");
            return new List<ServiceDto>();
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
            _notificationService.ShowError($"Ошибка при создании заявки: {ex.Message}");
            return false;
        }
    }
    
    public async Task<HttpResponseMessage> SaveOrder(OrderDto orderDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            var saveOrderDto = new SaveOrderCommand() { OrderDto = orderDto };
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _httpClient.PostAsJsonAsync($"api/v1/order/SaveOrder", saveOrderDto);
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при сохранении заявки: {ex.Message}");
            return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        }
    }

    public async Task<bool> ChangeStatusToWaitingInvoice(ChangeStatusToWaitingInvoiceDto changeStatusToWaitingInvoiceDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/v1/order/ChangeStatusToWaitingInvoice", changeStatusToWaitingInvoiceDto);

            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при изменении статуса заявки: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> ChangeOrderStatus(ChangeOrderStatusCommand changeOrderStatusCommand)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/v1/order/ChangeOrderStatus", changeOrderStatusCommand);

            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при изменении статуса заявки: {ex.Message}");
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
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при блокировке заявки: {ex.Message}");
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

    public async Task<List<CarDto>> GetAllActiveCars()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var cars = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/v1/car/GetActiveCars");
            return cars ?? new List<CarDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке автомобилей: {ex.Message}");
            return new List<CarDto>();
        }
    }

    public async Task<List<GetOrdersByFilterResponse>> GetOrdersByFilter(GetOrdersByFilterRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>();

            if (request.DateFrom.HasValue)
            {
                queryParams.Add($"DateFrom={request.DateFrom.Value:O}");
            }

            if (request.DateTo.HasValue)
            {
                queryParams.Add($"DateTo={request.DateTo.Value:O}");
            }

            if (!string.IsNullOrWhiteSpace(request.ClientSearchTerm))
            {
                queryParams.Add($"ClientSearchTerm={Uri.EscapeDataString(request.ClientSearchTerm)}");
            }

            if (request.Statuses != null && request.Statuses.Any())
            {
                foreach (var status in request.Statuses)
                {
                    queryParams.Add($"Statuses={status}");
                }
            }

            if (request.PaymentTypes != null && request.PaymentTypes.Any())
            {
                foreach (var paymentType in request.PaymentTypes)
                {
                    queryParams.Add($"PaymentTypes={paymentType}");
                }
            }

            if (request.ServiceIds != null && request.ServiceIds.Any())
            {
                foreach (var serviceId in request.ServiceIds)
                {
                    queryParams.Add($"ServiceIds={serviceId}");
                }
            }

            if (request.CreatedEmployeeIds != null && request.CreatedEmployeeIds.Any())
            {
                foreach (var employeeId in request.CreatedEmployeeIds)
                {
                    queryParams.Add($"CreatedEmployeeIds={employeeId}");
                }
            }

            var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : string.Empty;
            var url = $"api/v1/order/GetOrdersByFilter{queryString}";

            var orders = await _httpClient.GetFromJsonAsync<List<GetOrdersByFilterResponse>>(url);
            return orders ?? new List<GetOrdersByFilterResponse>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при фильтрации заявок: {ex.Message}");
            return new List<GetOrdersByFilterResponse>();
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
            _notificationService.ShowError($"Ошибка при загрузке документа: {ex.Message}");
            return false;
        }
    }

    public async Task<List<EmployeeEarningByFilterDto>> GetEmployeeEarningsByMonth(GetEmployeeEarningByMonthRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"Year={request.Year}",
                $"Month={request.Month}"
            };

            if (request.DateFrom.HasValue)
            {
                queryParams.Add($"DateFrom={request.DateFrom.Value:O}");
            }

            if (request.DateTo.HasValue)
            {
                queryParams.Add($"DateTo={request.DateTo.Value:O}");
            }

            if (!string.IsNullOrWhiteSpace(request.ClientSearchTerm))
            {
                queryParams.Add($"ClientSearchTerm={Uri.EscapeDataString(request.ClientSearchTerm)}");
            }

            if (request.EmployeeIds != null && request.EmployeeIds.Any())
            {
                foreach (var employeeId in request.EmployeeIds)
                {
                    queryParams.Add($"EmployeeIds={employeeId}");
                }
            }

            if (request.ServiceIds != null && request.ServiceIds.Any())
            {
                foreach (var serviceId in request.ServiceIds)
                {
                    queryParams.Add($"ServiceIds={serviceId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EmployeeEarning/GetEmployeeEarningByMounth{queryString}";

            var earnings = await _httpClient.GetFromJsonAsync<List<EmployeeEarningByFilterDto>>(url);
            return earnings ?? new List<EmployeeEarningByFilterDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков сотрудников: {ex.Message}");
            return new List<EmployeeEarningByFilterDto>();
        }
    }

    public async Task<List<DispetcherEarningByFilterDto>> GetDispetcherEarningsByMonth(GetDispetcherEarningByMounthRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"Year={request.Year}",
                $"Month={request.Month}"
            };

            if (request.DateFrom.HasValue)
            {
                queryParams.Add($"DateFrom={request.DateFrom.Value:O}");
            }

            if (request.DateTo.HasValue)
            {
                queryParams.Add($"DateTo={request.DateTo.Value:O}");
            }

            if (!string.IsNullOrWhiteSpace(request.ClientSearchTerm))
            {
                queryParams.Add($"ClientSearchTerm={Uri.EscapeDataString(request.ClientSearchTerm)}");
            }

            if (request.DispetcherIds != null && request.DispetcherIds.Any())
            {
                foreach (var dispetcherId in request.DispetcherIds)
                {
                    queryParams.Add($"DispetcherIds={dispetcherId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/DispetcherEarning/GetDispetcherEarningByMounth{queryString}";

            var earnings = await _httpClient.GetFromJsonAsync<List<DispetcherEarningByFilterDto>>(url);
            return earnings ?? new List<DispetcherEarningByFilterDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков диспетчеров: {ex.Message}");
            return new List<DispetcherEarningByFilterDto>();
        }
    }

    public async Task<bool> UpdateEmployeeEarning(EmployeeEarningUpdateCommand request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/v1/EmployeeEarning/UpdateEmployeeEarning", request);

            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении заработка сотрудника: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateDispetcherEarning(DispetcherEarningUpdateCommand request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _httpClient.PostAsJsonAsync("api/v1/DispetcherEarning/UpdateDispetcherEarning", request);

            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении заработка диспетчера: {ex.Message}");
            return false;
        }
    }

    // Car CRUD operations
    public async Task<List<CarDto>> GetCars()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var cars = await _httpClient.GetFromJsonAsync<List<CarDto>>("api/v1/car/GetCars");
            return cars ?? new List<CarDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке автомобилей: {ex.Message}");
            return new List<CarDto>();
        }
    }

    public async Task<bool> CreateCar(CarDto carDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateCarRequest
            {
                Number = carDto.Number,
                Status = carDto.Status,
                CarModelId = carDto.CarModel?.Id ?? 0
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/car/CreateCar", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании автомобиля: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateCar(CarDto carDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateCarCommand
            {
                Id = carDto.Id,
                Number = carDto.Number,
                Status = carDto.Status,
                CarModelId = carDto.CarModel?.Id ?? 0
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/car/UpdateCar", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении автомобиля: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCar(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteCarCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/car/DeleteCar")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении автомобиля: {ex.Message}");
            return false;
        }
    }

    // CarModel CRUD operations
    public async Task<List<CarModelDto>> GetCarModels()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var carModels = await _httpClient.GetFromJsonAsync<List<CarModelDto>>("api/v1/carmodel/GetCarModels");
            return carModels ?? new List<CarModelDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке моделей автомобилей: {ex.Message}");
            return new List<CarModelDto>();
        }
    }

    public async Task<bool> CreateCarModel(CarModelDto carModelDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateCarModelRequest { Name = carModelDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/carmodel/CreateCarModel", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании модели автомобиля: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateCarModel(CarModelDto carModelDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateCarModelCommand { Id = carModelDto.Id, Name = carModelDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/carmodel/UpdateCarModel", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении модели автомобиля: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteCarModel(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteCarModelCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/carmodel/DeleteCarModel")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении модели автомобиля: {ex.Message}");
            return false;
        }
    }

    // Employee CRUD operations
    public async Task<List<EmployeeDto>> GetEmployees()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var employees = await _httpClient.GetFromJsonAsync<List<EmployeeDto>>("api/v1/employee/GetEmployees");
            return employees ?? new List<EmployeeDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке сотрудников: {ex.Message}");
            return new List<EmployeeDto>();
        }
    }

    public async Task<bool> CreateEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateEmployeeRequest
            {
                Name = employeeDto.Name,
                Phone = null,
                Login = employeeDto.Login ?? "",
                Password = employeeDto.Password ?? "",
                DispetcherProcent = employeeDto.DispetcherProcent,
                RoleId = employeeDto.Role.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/employee/CreateEmployee", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании сотрудника: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEmployee(EmployeeDto employeeDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateEmployeeCommand
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Phone = null,
                Login = employeeDto.Login ?? "",
                Password = employeeDto.Password ?? "",
                DispetcherProcent = employeeDto.DispetcherProcent,
                RoleId = employeeDto.Role.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/employee/UpdateEmployee", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении сотрудника: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteEmployeeCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/employee/DeleteEmployee")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении сотрудника: {ex.Message}");
            return false;
        }
    }

    // Service CRUD operations
    public async Task<bool> CreateService(ServiceDto serviceDto, int measureId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateServiceRequest
            {
                Name = serviceDto.Name,
                Price = serviceDto.MinPrice,
                MeasureId = measureId,
                MinVolume = serviceDto.MinVolume,
                StandartPrecentForEmployee = serviceDto.StandartPrecentForEmployee,
                PrecentForMultipleEmployeers = serviceDto.PrecentForMultipleEmployeers,
                PrecentLaterOrderForEmployee = serviceDto.PrecentLaterOrderForEmployee,
                PrecentLaterOrderForMultipleEmployeers = serviceDto.PrecentLaterOrderForMultipleEmployeers
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/service/CreateService", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании услуги: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateService(ServiceDto serviceDto, int measureId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateServiceCommand
            {
                Id = serviceDto.Id,
                Name = serviceDto.Name,
                Price = serviceDto.MinPrice,
                MeasureId = measureId,
                MinVolume = serviceDto.MinVolume,
                StandartPrecentForEmployee = serviceDto.StandartPrecentForEmployee,
                PrecentForMultipleEmployeers = serviceDto.PrecentForMultipleEmployeers,
                PrecentLaterOrderForEmployee = serviceDto.PrecentLaterOrderForEmployee,
                PrecentLaterOrderForMultipleEmployeers = serviceDto.PrecentLaterOrderForMultipleEmployeers
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/service/UpdateService", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении услуги: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteService(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteServiceCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/service/DeleteService")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении услуги: {ex.Message}");
            return false;
        }
    }

    // Role CRUD operations
    public async Task<List<RoleDto>> GetRoles()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>("api/v1/role/GetRoles");
            return roles ?? new List<RoleDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке ролей: {ex.Message}");
            return new List<RoleDto>();
        }
    }

    public async Task<bool> CreateRole(RoleDto roleDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateRoleRequest { Name = roleDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/role/CreateRole", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании роли: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateRole(RoleDto roleDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateRoleCommand { Id = roleDto.Id, Name = roleDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/role/UpdateRole", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении роли: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteRole(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteRoleCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/role/DeleteRole")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении роли: {ex.Message}");
            return false;
        }
    }

    // Measure CRUD operations
    public async Task<List<MeasureDto>> GetMeasures()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var measures = await _httpClient.GetFromJsonAsync<List<MeasureDto>>("api/v1/measure/GetMeasures");
            return measures ?? new List<MeasureDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке единиц измерения: {ex.Message}");
            return new List<MeasureDto>();
        }
    }

    public async Task<bool> CreateMeasure(MeasureDto measureDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new CreateMeasureRequest { Name = measureDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/measure/CreateMeasure", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при создании единицы измерения: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateMeasure(MeasureDto measureDto)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new UpdateMeasureCommand { Id = measureDto.Id, Name = measureDto.Name };
            var response = await _httpClient.PostAsJsonAsync("api/v1/measure/UpdateMeasure", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при обновлении единицы измерения: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteMeasure(int id)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteMeasureCommand { Id = id };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/measure/DeleteMeasure")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении единицы измерения: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteOrderCommand { Id = orderId };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/order/DeleteOrder")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении заявки: {ex.Message}");
            return false;
        }
    }
    
    public async Task<bool> DeleteDocument(int documentId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new DeleteDocumentCommand { Id = documentId };
            var request = new HttpRequestMessage(HttpMethod.Delete, "api/v1/document/DeleteDocument")
            {
                Content = JsonContent.Create(command)
            };
            
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при удалении документа: {ex.Message}");
            return false;
        }
    }

    public async Task<byte[]?> DownloadEmployeeEarningReport(List<EmployeeEarningByFilterDto> data)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new DownloadEmployeeEarningReportRequest { Data = data };
            var response = await _httpClient.PostAsJsonAsync("api/v1/EmployeeEarning/DownloadReport", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            _notificationService.ShowError("Ошибка при скачивании отчета");
            return null;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при скачивании отчета: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SendEmployeeEarningReportToTelegram(List<EmployeeEarningByFilterDto> data)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new SendEmployeeEarningReportToTelegramCommand { Data = data };
            var response = await _httpClient.PostAsJsonAsync("api/v1/EmployeeEarning/SendReportToTelegram", command);

            if (response.IsSuccessStatusCode)
            {
                _notificationService.ShowInfo("Отчет успешно отправлен в Telegram");
                return true;
            }

            _notificationService.ShowError("Ошибка при отправке отчета в Telegram");
            return false;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при отправке отчета: {ex.Message}");
            return false;
        }
    }

    public async Task<byte[]?> DownloadDispetcherEarningReport(List<DispetcherEarningByFilterDto> data)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var request = new DownloadDispetcherEarningReportRequest { Data = data };
            var response = await _httpClient.PostAsJsonAsync("api/v1/DispetcherEarning/DownloadReport", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            _notificationService.ShowError("Ошибка при скачивании отчета");
            return null;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при скачивании отчета: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SendDispetcherEarningReportToTelegram(List<DispetcherEarningByFilterDto> data)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new SendDispetcherEarningReportToTelegramCommand { Data = data };
            var response = await _httpClient.PostAsJsonAsync("api/v1/DispetcherEarning/SendReportToTelegram", command);

            if (response.IsSuccessStatusCode)
            {
                _notificationService.ShowInfo("Отчет успешно отправлен в Telegram");
                return true;
            }

            _notificationService.ShowError("Ошибка при отправке отчета в Telegram");
            return false;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при отправке отчета: {ex.Message}");
            return false;
        }
    }

    public async Task<List<CarEarningAnalyticsDto>> GetCarEarningAnalytics(GetCarEarningAnalyticsRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}"
            };

            if (request.CarIds != null && request.CarIds.Any())
            {
                foreach (var carId in request.CarIds)
                {
                    queryParams.Add($"CarIds={carId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/cars{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<CarEarningAnalyticsDto>>(url);
            return analytics ?? new List<CarEarningAnalyticsDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке аналитики по машинам: {ex.Message}");
            return new List<CarEarningAnalyticsDto>();
        }
    }

    public async Task<List<ServiceEarningAnalyticsDto>> GetServiceEarningAnalytics(GetServiceEarningAnalyticsRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}"
            };

            if (request.ServiceIds != null && request.ServiceIds.Any())
            {
                foreach (var serviceId in request.ServiceIds)
                {
                    queryParams.Add($"ServiceIds={serviceId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/services{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<ServiceEarningAnalyticsDto>>(url);
            return analytics ?? new List<ServiceEarningAnalyticsDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке аналитики по услугам: {ex.Message}");
            return new List<ServiceEarningAnalyticsDto>();
        }
    }

    public async Task<List<DriverEarningAnalyticsDto>> GetDriverEarningAnalytics(GetDriverEarningAnalyticsRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}"
            };

            if (request.DriverIds != null && request.DriverIds.Any())
            {
                foreach (var driverId in request.DriverIds)
                {
                    queryParams.Add($"DriverIds={driverId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/drivers{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<DriverEarningAnalyticsDto>>(url);
            return analytics ?? new List<DriverEarningAnalyticsDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке аналитики по водителям: {ex.Message}");
            return new List<DriverEarningAnalyticsDto>();
        }
    }

    public async Task<List<DispatcherEarningAnalyticsDto>> GetDispatcherEarningAnalytics(GetDispatcherEarningAnalyticsRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            // Построение URL с query параметрами
            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}"
            };

            if (request.DispatcherIds != null && request.DispatcherIds.Any())
            {
                foreach (var dispatcherId in request.DispatcherIds)
                {
                    queryParams.Add($"DispatcherIds={dispatcherId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/dispatchers{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<DispatcherEarningAnalyticsDto>>(url);
            return analytics ?? new List<DispatcherEarningAnalyticsDto>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке аналитики по диспетчерам: {ex.Message}");
            return new List<DispatcherEarningAnalyticsDto>();
        }
    }

    public async Task<List<ServiceEarningByPeriodDto>> GetServiceEarningByPeriod(GetServiceEarningByPeriodRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"GroupPeriod={((int)request.GroupPeriod)}"
            };

            if (request.ServiceIds != null && request.ServiceIds.Any())
            {
                foreach (var serviceId in request.ServiceIds)
                {
                    queryParams.Add($"ServiceIds={serviceId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/service-earnings-by-period{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<ServiceEarningByPeriodDto>>(url);
            return analytics ?? new List<ServiceEarningByPeriodDto>();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке аналитики по периодам: {ex.Message}");
            return new List<ServiceEarningByPeriodDto>();
        }
    }

    public async Task<List<CarEarningByPeriodDto>> GetCarEarningByPeriod(GetCarEarningByPeriodRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"GroupPeriod={((int)request.GroupPeriod)}"
            };

            if (request.CarIds != null && request.CarIds.Any())
            {
                foreach (var carId in request.CarIds)
                {
                    queryParams.Add($"CarIds={carId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/car-earnings-by-period{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<CarEarningByPeriodDto>>(url);
            return analytics ?? new List<CarEarningByPeriodDto>();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков машин по периодам: {ex.Message}");
            return new List<CarEarningByPeriodDto>();
        }
    }

    public async Task<List<DriverEarningByPeriodDto>> GetDriverEarningByPeriod(GetDriverEarningByPeriodRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"GroupPeriod={((int)request.GroupPeriod)}"
            };

            if (request.DriverIds != null && request.DriverIds.Any())
            {
                foreach (var driverId in request.DriverIds)
                {
                    queryParams.Add($"DriverIds={driverId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/driver-earnings-by-period{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<DriverEarningByPeriodDto>>(url);
            return analytics ?? new List<DriverEarningByPeriodDto>();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков водителей по периодам: {ex.Message}");
            return new List<DriverEarningByPeriodDto>();
        }
    }

    public async Task<List<DispatcherEarningByPeriodDto>> GetDispatcherEarningByPeriod(GetDispatcherEarningByPeriodRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"GroupPeriod={((int)request.GroupPeriod)}"
            };

            if (request.DispatcherIds != null && request.DispatcherIds.Any())
            {
                foreach (var dispatcherId in request.DispatcherIds)
                {
                    queryParams.Add($"DispatcherIds={dispatcherId}");
                }
            }

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/dispatcher-earnings-by-period{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<DispatcherEarningByPeriodDto>>(url);
            return analytics ?? new List<DispatcherEarningByPeriodDto>();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков диспетчеров по периодам: {ex.Message}");
            return new List<DispatcherEarningByPeriodDto>();
        }
    }

    public async Task<List<ZasNetEarningByPeriodDto>> GetZasNetEarning(GetZasNetEarningRequest request)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var queryParams = new List<string>
            {
                $"DateFrom={Uri.EscapeDataString(request.DateFrom.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"DateTo={Uri.EscapeDataString(request.DateTo.ToString("yyyy-MM-ddTHH:mm:ss"))}",
                $"GroupPeriod={((int)request.GroupPeriod)}"
            };

            var queryString = "?" + string.Join("&", queryParams);
            var url = $"api/v1/EarningAnalytics/zasnet-earning{queryString}";

            var analytics = await _httpClient.GetFromJsonAsync<List<ZasNetEarningByPeriodDto>>(url);
            return analytics ?? new List<ZasNetEarningByPeriodDto>();
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заработков компании: {ex.Message}");
            return new List<ZasNetEarningByPeriodDto>();
        }
    }

    public async Task<List<GetLockedOrdersResponse>> GetLockedOrders()
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");
            
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var lockedOrders = await _httpClient.GetFromJsonAsync<List<GetLockedOrdersResponse>>("api/v1/order/GetLockedOrders");
            return lockedOrders ?? new List<GetLockedOrdersResponse>();
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при загрузке заблокированных заявок: {ex.Message}");
            return new List<GetLockedOrdersResponse>();
        }
    }

    public async Task<bool> ResetLockedOrder(int orderId)
    {
        try
        {
            var token = await _localStorageService.GetItemAsync<string>("token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var command = new ResetLocksCommand { OrderId = orderId };
            var response = await _httpClient.PostAsJsonAsync("api/v1/order/ResetLockedOrder", command);

            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка при сбросе блокировки заявки: {ex.Message}");
            return false;
        }
    }
}

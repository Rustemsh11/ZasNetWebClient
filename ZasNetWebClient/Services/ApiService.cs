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
            return false;
        }
    }
    
    public async Task<HttpResponseMessage> SaveOrder(OrderDto orderDto)
    {
        var token = await _localStorageService.GetItemAsync<string>("token");
        var saveOrderDto = new SaveOrderCommand() { OrderDto = orderDto };
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await _httpClient.PostAsJsonAsync($"api/v1/order/SaveOrder", saveOrderDto);
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
        catch
        {
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
            Console.WriteLine($"Error getting orders by filter: {ex.Message}");
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
            return false;
        }
    }

    public async Task<List<GetEmployeeEarningByMonthResponse>> GetEmployeeEarningsByMonth(GetEmployeeEarningByMonthRequest request)
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

            var earnings = await _httpClient.GetFromJsonAsync<List<GetEmployeeEarningByMonthResponse>>(url);
            return earnings ?? new List<GetEmployeeEarningByMonthResponse>();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error getting employee earnings: {ex.Message}");
            return new List<GetEmployeeEarningByMonthResponse>();
        }
    }

    public async Task<List<GetDispetcherEarningByMounthResponse>> GetDispetcherEarningsByMonth(GetDispetcherEarningByMounthRequest request)
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

            var earnings = await _httpClient.GetFromJsonAsync<List<GetDispetcherEarningByMounthResponse>>(url);
            return earnings ?? new List<GetDispetcherEarningByMounthResponse>();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error getting dispetcher earnings: {ex.Message}");
            return new List<GetDispetcherEarningByMounthResponse>();
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
            Console.WriteLine($"Error updating employee earning: {ex.Message}");
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
            Console.WriteLine($"Error updating dispatcher earning: {ex.Message}");
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
            Console.WriteLine($"Error getting cars: {ex.Message}");
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
                CarModelId = carDto.CarModel.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/car/CreateCar", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error creating car: {ex.Message}");
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
                CarModelId = carDto.CarModel.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/car/UpdateCar", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error updating car: {ex.Message}");
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
            Console.WriteLine($"Error deleting car: {ex.Message}");
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
            Console.WriteLine($"Error getting car models: {ex.Message}");
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
            Console.WriteLine($"Error creating car model: {ex.Message}");
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
            Console.WriteLine($"Error updating car model: {ex.Message}");
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
            Console.WriteLine($"Error deleting car model: {ex.Message}");
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
                RoleId = employeeDto.Role.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/employee/CreateEmployee", request);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error creating employee: {ex.Message}");
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
                RoleId = employeeDto.Role.Id
            };

            var response = await _httpClient.PostAsJsonAsync("api/v1/employee/UpdateEmployee", command);
            return response.IsSuccessStatusCode;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error updating employee: {ex.Message}");
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
            Console.WriteLine($"Error deleting employee: {ex.Message}");
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
            Console.WriteLine($"Error creating service: {ex.Message}");
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
            Console.WriteLine($"Error updating service: {ex.Message}");
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
            Console.WriteLine($"Error deleting service: {ex.Message}");
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
            Console.WriteLine($"Error getting roles: {ex.Message}");
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
            Console.WriteLine($"Error creating role: {ex.Message}");
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
            Console.WriteLine($"Error updating role: {ex.Message}");
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
            Console.WriteLine($"Error deleting role: {ex.Message}");
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
            Console.WriteLine($"Error getting measures: {ex.Message}");
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
            Console.WriteLine($"Error creating measure: {ex.Message}");
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
            Console.WriteLine($"Error updating measure: {ex.Message}");
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
            Console.WriteLine($"Error deleting measure: {ex.Message}");
            return false;
        }
    }
}

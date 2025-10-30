using ZasNetWebClient.Models;
using System.Net.Http.Json;

namespace ZasNetWebClient.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public ApiService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
        
        // Set up authorization header
        _authService.OnAuthenticationStateChanged += UpdateAuthorizationHeader;
        // Apply header immediately if token already present
        UpdateAuthorizationHeader();
    }
    
    private void UpdateAuthorizationHeader()
    {
        var token = _authService.GetToken();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    public async Task<List<Application>> GetAllApplicationsAsync()
    {
        try
        {
            // For demo purposes, return mock data
            // In a real app, this would call your API
            await Task.Delay(300); // Simulate API call
            
            return new List<Application>
            {
                new Application
                {
                    Id = 1,
                    Title = "Website Redesign Project",
                    Description = "Complete redesign of the company website with modern UI/UX",
                    Status = "In Progress",
                    CreatedDate = DateTime.Now.AddDays(-5),
                    UpdatedDate = DateTime.Now.AddDays(-1),
                    ManagerId = 1,
                    ManagerName = "John Manager",
                    Priority = "High",
                    Category = "Web Development"
                },
                new Application
                {
                    Id = 2,
                    Title = "Mobile App Development",
                    Description = "Development of a new mobile application for iOS and Android",
                    Status = "Pending",
                    CreatedDate = DateTime.Now.AddDays(-3),
                    ManagerId = 1,
                    ManagerName = "John Manager",
                    Priority = "Medium",
                    Category = "Mobile Development"
                },
                new Application
                {
                    Id = 3,
                    Title = "Database Migration",
                    Description = "Migration from legacy database to modern cloud solution",
                    Status = "Completed",
                    CreatedDate = DateTime.Now.AddDays(-10),
                    UpdatedDate = DateTime.Now.AddDays(-2),
                    ManagerId = 1,
                    ManagerName = "John Manager",
                    Priority = "High",
                    Category = "Infrastructure"
                },
                new Application
                {
                    Id = 4,
                    Title = "Security Audit",
                    Description = "Comprehensive security audit of all systems",
                    Status = "In Review",
                    CreatedDate = DateTime.Now.AddDays(-7),
                    UpdatedDate = DateTime.Now.AddDays(-1),
                    ManagerId = 1,
                    ManagerName = "John Manager",
                    Priority = "Critical",
                    Category = "Security"
                }
            };
        }
        catch
        {
            return new List<Application>();
        }
    }

    public async Task<List<Application>> GetMyApplicationsAsync()
    {
        try
        {
            // For demo purposes, return applications assigned to current manager
            var allApplications = await GetAllApplicationsAsync();
            var currentUserId = _authService.CurrentUser?.Id ?? 0;
            if (currentUserId <= 0)
            {
                return allApplications; // No Id available; show all in demo
            }
            return allApplications.Where(app => app.ManagerId == currentUserId).ToList();
        }
        catch
        {
            return new List<Application>();
        }
    }

    public async Task<bool> CreateApplicationAsync(CreateApplicationDto application)
    {
        try
        {
            // For demo purposes, simulate API call
            await Task.Delay(1000); // Simulate API call
            
            // In a real app, this would be:
            // var response = await _httpClient.PostAsJsonAsync("/api/applications", application);
            // return response.IsSuccessStatusCode;
            
            return true; // Mock success
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ServiceDto>> GetServicesAsync()
    {
        try
        {
            // Mock data - in real app, this would call your API
            await Task.Delay(200);
            
            return new List<ServiceDto>
            {
                new ServiceDto { Id = 1, Name = "Cleaning Service", Description = "General cleaning", BasePrice = 50, Unit = "hour" },
                new ServiceDto { Id = 2, Name = "Maintenance", Description = "Equipment maintenance", BasePrice = 75, Unit = "hour" },
                new ServiceDto { Id = 3, Name = "Installation", Description = "Equipment installation", BasePrice = 100, Unit = "hour" },
                new ServiceDto { Id = 4, Name = "Consultation", Description = "Technical consultation", BasePrice = 60, Unit = "hour" },
                new ServiceDto { Id = 5, Name = "Repair", Description = "Equipment repair", BasePrice = 80, Unit = "hour" }
            };
        }
        catch
        {
            return new List<ServiceDto>();
        }
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        try
        {
            // Mock data - in real app, this would call your API
            await Task.Delay(200);
            
            return new List<EmployeeDto>
            {
                new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe", Position = "Technician" },
                new EmployeeDto { Id = 2, FirstName = "Jane", LastName = "Smith", Position = "Manager" },
                new EmployeeDto { Id = 3, FirstName = "Bob", LastName = "Johnson", Position = "Specialist" },
                new EmployeeDto { Id = 4, FirstName = "Alice", LastName = "Brown", Position = "Technician" }
            };
        }
        catch
        {
            return new List<EmployeeDto>();
        }
    }

    public async Task<List<CarDto>> GetCarsAsync()
    {
        try
        {
            // Mock data - in real app, this would call your API
            await Task.Delay(200);
            
            return new List<CarDto>
            {
                new CarDto { Id = 1, Make = "Ford", Model = "Transit", LicensePlate = "ABC-123" },
                new CarDto { Id = 2, Make = "Mercedes", Model = "Sprinter", LicensePlate = "DEF-456" },
                new CarDto { Id = 3, Make = "Volkswagen", Model = "Crafter", LicensePlate = "GHI-789" }
            };
        }
        catch
        {
            return new List<CarDto>();
        }
    }
}

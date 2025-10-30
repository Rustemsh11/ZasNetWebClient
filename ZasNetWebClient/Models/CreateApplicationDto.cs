namespace ZasNetWebClient.Models;

public enum PaymentType
{
    Cash,
    Card,
    BankTransfer,
    Check
}

public class OrderServicesDto
{
    public int ServiceId { get; set; }
    public decimal Price { get; set; }
    public double TotalVolume { get; set; }
}


public class CreateApplicationDto
{
    public string Client { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string AddressCity { get; set; } = string.Empty;
    public string AddressStreet { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public decimal OrderPriceAmount { get; set; }
    public PaymentType PaymentType { get; set; } = PaymentType.Cash;
    public string? Description { get; set; }
    public List<int> OrderEmployeeIds { get; set; } = new();
    public List<int> OrderCarIds { get; set; } = new();
    public List<OrderServicesDto> OrderServicesDto { get; set; } = new();
}

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Unit { get; set; } = string.Empty;
}

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
}

public class CarDto
{
    public int Id { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
}

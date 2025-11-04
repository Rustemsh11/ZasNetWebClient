namespace ZasNetWebClient.Models;

public enum PaymentType
{
    None = 0,
    Cash = 1,
    Electronic = 2,
    Card = 3,
}

public enum ClientType
{
    FizNal = 0,
    LegalWithVat = 1,
    LegalWithoutVat = 2,
}

public enum OrderStatus
{
    Created = 0,
    Processing = 1,
    Finished = 2,
    CreatedInvoice = 3,
    AwaitingPayment = 4,
    Closed = 5,
}

public class OrderServicesDto
{
    public int ServiceId { get; set; }
    public decimal Price { get; set; }
    public double TotalVolume { get; set; }
}


public class OrderDto
{
    public string Client { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string AddressCity { get; set; } = string.Empty;
    public string AddressStreet { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public decimal OrderPriceAmount { get; set; }
    public PaymentType PaymentType { get; set; } = PaymentType.Cash;
    public ClientType ClientType { get; set; } = ClientType.FizNal;
    public string? Description { get; set; }
    public int UserId { get; set; }
    public List<int> OrderEmployeeIds { get; set; } = new();
    public List<int> OrderCarIds { get; set; } = new();
    public List<OrderServicesDto> OrderServicesDto { get; set; } = new();
}

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal MinPrice { get; set; }
    public double MinVolume { get; set; }
    public string Measure { get; set; } = string.Empty;
}

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CarDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateOrderParameters
{
    public List<ServiceDto> ServiceDtos { get; set; }
    public List<EmployeeDto> EmployeeDtos { get; set; } = new();
    public List<CarDto> CarDtos { get; set; } = new();
    public List<PaymentType> PaymentTypes { get; set; } = new();
}
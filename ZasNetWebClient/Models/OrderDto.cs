using System.ComponentModel;

namespace ZasNetWebClient.Models;

public enum PaymentType
{
    None = 0,
    Cash = 1,
    Electronic = 2,
    Card = 3,
    CashWithVat = 4,
    CashWithoutVat = 5,
}

public enum OrderStatus
{
    Created = 0,
    ApprovedWithEmployers = 1,
    Processing = 2,
    Finished = 3,
    CreatingInvoice = 4,
    AwaitingPayment = 5,
    Closed = 6,
}

public class OrderServiceDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public decimal Price { get; set; }
    public double TotalVolume { get; set; }
    public List<OrderServiceEmployeeDto> OrderServiceEmployeeDtos { get; set; } = new();
    public List<OrderServiceCarDto> OrderServiceCarDtos { get; set; } = new();
}


public class OrderDto
{
    public int Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public DateTime DateStart { get; set; } = DateTime.Now;
    public DateTime DateEnd { get; set; } = DateTime.Now;
    public string AddressCity { get; set; } = string.Empty;
    public string AddressStreet { get; set; } = string.Empty;
    public string AddressNumber { get; set; } = string.Empty;
    public decimal OrderPriceAmount { get; set; }
    public PaymentType PaymentType { get; set; } = PaymentType.Cash;
    public OrderStatus Status { get; set; }
    public string? Description { get; set; }
    public EmployeeDto CreatedUser { get; set; } = new();
    public List<OrderServiceDto> OrderServicesDtos { get; set; } = new();
    public List<DocumentDto> Documents { get; set; } = new();
}

public class OrderServiceEmployeeDto
{
    public int OrderServiceId { get; set; }
    public EmployeeDto Employee { get; set; } = new();
    public bool IsApproved {get; set;}
}

public class OrderServiceCarDto
{
    public int OrderServiceId { get; set; }
    public CarDto Car { get; set; } = new();
    public bool IsApproved {get; set;}
}

public class DocumentDto
{
    public int Id { get; set; }
	public string Name { get; set; }
	public string Extension { get; set; }
	public string? ContentType { get; set; }
	public long? SizeBytes { get; set; }
	public DateTime UploadedDate { get; set; }
	public DocumentType DocumentType { get; set; }

	// Relative URLs to be used by the web client
	public string ViewUrl { get; set; }
	public string DownloadUrl { get; set; }

    public bool IsImage => (ContentType ?? string.Empty).StartsWith("image/", StringComparison.OrdinalIgnoreCase);
}

public enum DocumentType
{
    None = 0,

    Invoice = 1,

    ActOfCompletedWorks = 2,

    WorkReport = 3,
}

public class CreateOrderCommand
{
    public OrderDto OrderDto { get; set; } = new();
}

public class SaveOrderCommand
{
    public OrderDto OrderDto { get; set; } = new();
    public int UserId;
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
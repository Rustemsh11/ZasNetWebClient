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
    SendingPayment = 5,
    AwaitingPayment = 6,
    Closed = 7,
}

public class OrderServiceDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public decimal Price { get; set; }
    public double TotalVolume { get; set; }
    public decimal StandartPrecentForEmployee { get; set; }
    public decimal PrecentForMultipleEmployeers { get; set; }
    public decimal PrecentLaterOrderForEmployee { get; set; }
    public decimal PrecentLaterOrderForMultipleEmployeers { get; set; }
    public bool IsAlmazService { get; set; }
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
    public bool IsCashWasTransferred { get; set; }
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
    public int MeasureId { get; set; }

    public decimal StandartPrecentForEmployee { get; set; }

    public decimal PrecentForMultipleEmployeers { get; set; }

    public decimal PrecentLaterOrderForEmployee { get; set; }

    public decimal PrecentLaterOrderForMultipleEmployeers { get; set; }
}

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Login { get; set; }
    public string? Password { get; set; }
    public decimal? DispetcherProcent { get; set; }
    public RoleDto Role { get; set; } = new();
}

public class CarDto
{
    public int Id { get; set; }
    public string Name => $"{this.Number} ({this.CarModel?.Name})";
    public string Number { get; set; } = string.Empty;
    public int Status { get; set; }
    public CarModelDto? CarModel { get; set; } = new();
}

public class CarModelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class MeasureDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

// Request/Command models for API
public class CreateCarRequest
{
    public string Number { get; set; } = string.Empty;
    public int Status { get; set; }
    public int? CarModelId { get; set; }
}

public class UpdateCarCommand
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int Status { get; set; }
    public int? CarModelId { get; set; }
}

public class DeleteCarCommand
{
    public int Id { get; set; }
}

public class CreateCarModelRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateCarModelCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class DeleteCarModelCommand
{
    public int Id { get; set; }
}

public class CreateEmployeeRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public decimal? DispetcherProcent { get; set; }
    public int RoleId { get; set; }
}

public class UpdateEmployeeCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public decimal? DispetcherProcent { get; set; }
    public int RoleId { get; set; }
}

public class DeleteEmployeeCommand
{
    public int Id { get; set; }
}

public class CreateServiceRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MeasureId { get; set; }
    public double MinVolume { get; set; }
    public decimal StandartPrecentForEmployee { get; set; }
    public decimal PrecentForMultipleEmployeers { get; set; }
    public decimal PrecentLaterOrderForEmployee { get; set; }
    public decimal PrecentLaterOrderForMultipleEmployeers { get; set; }
}

public class UpdateServiceCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int MeasureId { get; set; }
    public double MinVolume { get; set; }
    public decimal StandartPrecentForEmployee { get; set; }
    public decimal PrecentForMultipleEmployeers { get; set; }
    public decimal PrecentLaterOrderForEmployee { get; set; }
    public decimal PrecentLaterOrderForMultipleEmployeers { get; set; }
}

public class DeleteServiceCommand
{
    public int Id { get; set; }
}

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateRoleCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class DeleteRoleCommand
{
    public int Id { get; set; }
}

public class CreateMeasureRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateMeasureCommand
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class DeleteMeasureCommand
{
    public int Id { get; set; }
}

public class CreateOrderParameters
{
    public List<ServiceDto> ServiceDtos { get; set; }
    public List<EmployeeDto> EmployeeDtos { get; set; } = new();
    public List<CarDto> CarDtos { get; set; } = new();
    public List<PaymentType> PaymentTypes { get; set; } = new();
}

/// <summary>
/// Запрос на получение заявок с фильтрацией
/// </summary>
public class GetOrdersByFilterRequest
{
    /// <summary>
    /// Дата начала диапазона
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Дата окончания диапазона
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Список статусов для фильтрации
    /// </summary>
    public List<OrderStatus>? Statuses { get; set; }

    /// <summary>
    /// Поисковый запрос по имени клиента
    /// </summary>
    public string? ClientSearchTerm { get; set; }

    /// <summary>
    /// Список типов оплаты для фильтрации
    /// </summary>
    public List<PaymentType>? PaymentTypes { get; set; }

    /// <summary>
    /// Список ID услуг для фильтрации
    /// </summary>
    public List<int>? ServiceIds { get; set; }

    /// <summary>
    /// Список ID сотрудников, создавших заявку
    /// </summary>
    public List<int>? CreatedEmployeeIds { get; set; }
}

/// <summary>
/// Ответ с данными заявки (упрощенная версия)
/// </summary>
public class GetOrdersByFilterResponse
{
    public int Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Address { get; set; } = string.Empty;
    public decimal OrderPriceAmount { get; set; }
    public PaymentType PaymentType { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedEmployeeName { get; set; } = string.Empty;
    public List<string> ServiceNames { get; set; } = new();
    public List<string> CarNames { get; set; } = new();
}

/// <summary>
/// Запрос на получение заработков сотрудников за месяц с фильтрацией
/// </summary>
public class GetEmployeeEarningByMonthRequest
{
    /// <summary>
    /// Год (обязательный)
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Месяц (обязательный, 1-12)
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Список ID сотрудников для фильтрации
    /// </summary>
    public List<int>? EmployeeIds { get; set; }

    /// <summary>
    /// Поисковый запрос по имени клиента
    /// </summary>
    public string? ClientSearchTerm { get; set; }

    /// <summary>
    /// Дата начала диапазона для дополнительной фильтрации по датам заявки
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Дата окончания диапазона для дополнительной фильтрации по датам заявки
    /// </summary>
    public DateTime? DateTo { get; set; }

    /// <summary>
    /// Список ID услуг для фильтрации
    /// </summary>
    public List<int>? ServiceIds { get; set; }
}

/// <summary>
/// Ответ с данными о заработке сотрудника
/// </summary>
public class EmployeeEarningByFilterDto
{
    /// <summary>
    /// ID заявки
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Клиент заявки
    /// </summary>
    public string Client { get; set; } = string.Empty;

    /// <summary>
    /// Дата начала заявки
    /// </summary>
    public DateTime OrderDateStart { get; set; }

    /// <summary>
    /// Дата окончания заявки
    /// </summary>
    public DateTime OrderDateEnd { get; set; }

    /// <summary>
    /// Название услуги
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Сотрудник
    /// </summary>
    public EmployeeDto Employee { get; set; } = new();

    public EmployeeEarningDto EmployeeEarningDto { get; set; }

    /// <summary>
    /// Общая стоимость услуги
    /// </summary>
    public decimal ServiceTotalPrice { get; set; }

    /// <summary>
    /// Объем работ
    /// </summary>
    public double TotalVolume { get; set; }
}

public class EmployeeEarningDto
{
    public int EmployeeEarningId { get; set; }

    public decimal ServiceEmployeePrecent { get; set; }

    public string? PrecentEmployeeDescription { get; set; }

    public decimal EmployeeEarning { get; set; }
}

/// <summary>
/// Запрос на получение заработков диспетчеров за месяц с фильтрацией
/// </summary>
public class GetDispetcherEarningByMounthRequest
{
    /// <summary>
    /// Год (обязательный)
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Месяц (обязательный, 1-12)
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Список ID диспетчеров для фильтрации
    /// </summary>
    public List<int>? DispetcherIds { get; set; }

    /// <summary>
    /// Поисковый запрос по имени клиента
    /// </summary>
    public string? ClientSearchTerm { get; set; }

    /// <summary>
    /// Дата начала диапазона для дополнительной фильтрации по датам заявки
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Дата окончания диапазона для дополнительной фильтрации по датам заявки
    /// </summary>
    public DateTime? DateTo { get; set; }
}

/// <summary>
/// Ответ с данными о заработке диспетчера
/// </summary>
public class DispetcherEarningByFilterDto
{
    /// <summary>
    /// ID записи о заработке
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID заявки
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Клиент заявки
    /// </summary>
    public string Client { get; set; } = string.Empty;

    /// <summary>
    /// Дата начала заявки
    /// </summary>
    public DateTime OrderDateStart { get; set; }

    /// <summary>
    /// Дата окончания заявки
    /// </summary>
    public DateTime OrderDateEnd { get; set; }

    /// <summary>
    /// Диспетчер
    /// </summary>
    public EmployeeDto Dispetcher { get; set; } = new();

    /// <summary>
    /// Процент для диспетчера
    /// </summary>
    public decimal DispetcherPrecent { get; set; }

    /// <summary>
    /// Описание процента
    /// </summary>
    public string? PrecentDispetcherDescription { get; set; }

    /// <summary>
    /// Заработок диспетчера
    /// </summary>
    public decimal DispetcherEarning { get; set; }

    /// <summary>
    /// Общая стоимость заявки
    /// </summary>
    public decimal OrderTotalPrice { get; set; }
}

/// <summary>
/// Запрос на обновление заработка сотрудника
/// </summary>
public class EmployeeEarningUpdateCommand

{
    public EmployeeEarningDto EmployeeEarningDto { get; set; }
}

/// <summary>
/// Запрос на обновление заработка диспетчера
/// </summary>
public class DispetcherEarningUpdateCommand
{
    public EmployeeEarningDto EmployeeEarningDto { get; set; }
}
namespace ZasNetWebClient.Models;

/// <summary>
/// Аналитика заработков по машинам
/// </summary>
public class CarEarningAnalyticsDto
{
    /// <summary>
    /// ID машины
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Номер машины
    /// </summary>
    public string CarNumber { get; set; } = string.Empty;

    /// <summary>
    /// Модель машины
    /// </summary>
    public string? CarModel { get; set; }

    /// <summary>
    /// Общий заработок водителей, работавших на этой машине
    /// </summary>
    public decimal TotalEarnings { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int OrdersCount { get; set; }
}

/// <summary>
/// Аналитика заработков по услугам
/// </summary>
public class ServiceEarningAnalyticsDto
{
    /// <summary>
    /// ID услуги
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// Название услуги
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// Общий заработок сотрудников за эту услугу
    /// </summary>
    public decimal TotalEmployeesEarnings { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int OrdersCount { get; set; }

    /// <summary>
    /// Количество выполненных услуг
    /// </summary>
    public int ServicesCount { get; set; }

    /// <summary>
    /// Общая стоимость услуг
    /// </summary>
    public decimal TotalZasNetEarning { get; set; }
}

/// <summary>
/// Аналитика заработков по водителям (сотрудникам)
/// </summary>
public class DriverEarningAnalyticsDto
{
    /// <summary>
    /// ID водителя
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Имя водителя
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// Общий заработок водителя
    /// </summary>
    public decimal TotalZasNetEarningByEmployee { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int OrdersCount { get; set; }
}

/// <summary>
/// Аналитика заработков по диспетчерам
/// </summary>
public class DispatcherEarningAnalyticsDto
{
    /// <summary>
    /// ID диспетчера
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Имя диспетчера
    /// </summary>
    public string EmployeeName { get; set; } = string.Empty;

    /// <summary>
    /// Общий заработок диспетчера
    /// </summary>
    public decimal TotalDispetcherEarnings { get; set; }

    public decimal TotalZasNetEarningFromDispetcher { get; set; }

    /// <summary>
    /// Количество заявок
    /// </summary>
    public int OrdersCount { get; set; }
}

/// <summary>
/// Общий DTO для аналитики заработков
/// </summary>
public class EarningAnalyticsDto
{
    /// <summary>
    /// Аналитика по машинам
    /// </summary>
    public List<CarEarningAnalyticsDto> Cars { get; set; } = new();

    /// <summary>
    /// Аналитика по услугам
    /// </summary>
    public List<ServiceEarningAnalyticsDto> Services { get; set; } = new();

    /// <summary>
    /// Аналитика по водителям
    /// </summary>
    public List<DriverEarningAnalyticsDto> Drivers { get; set; } = new();

    /// <summary>
    /// Аналитика по диспетчерам
    /// </summary>
    public List<DispatcherEarningAnalyticsDto> Dispatchers { get; set; } = new();

    /// <summary>
    /// Период аналитики
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Период аналитики
    /// </summary>
    public DateTime? DateTo { get; set; }
}

/// <summary>
/// Запрос на получение аналитики заработков по машинам
/// </summary>
public class GetCarEarningAnalyticsRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID машин (опционально)
    /// </summary>
    public List<int>? CarIds { get; set; }
}

/// <summary>
/// Запрос на получение аналитики заработков по услугам
/// </summary>
public class GetServiceEarningAnalyticsRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID услуг (опционально)
    /// </summary>
    public List<int>? ServiceIds { get; set; }
}

/// <summary>
/// Запрос на получение аналитики заработков по водителям
/// </summary>
public class GetDriverEarningAnalyticsRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID водителей (опционально)
    /// </summary>
    public List<int>? DriverIds { get; set; }
}

/// <summary>
/// Запрос на получение аналитики заработков по диспетчерам
/// </summary>
public class GetDispatcherEarningAnalyticsRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID диспетчеров (опционально)
    /// </summary>
    public List<int>? DispatcherIds { get; set; }
}

/// <summary>
/// Тип периода для группировки данных
/// </summary>
public enum GroupPeriod
{
    /// <summary>
    /// Группировка по дням
    /// </summary>
    Day = 1,

    /// <summary>
    /// Группировка по месяцам
    /// </summary>
    Month = 2,

    /// <summary>
    /// Группировка по годам
    /// </summary>
    Year = 3
}

/// <summary>
/// Прибыль услуги за период
/// </summary>
public class ServiceEarningByPeriodDto
{
    /// <summary>
    /// Период (дата для дня, первый день месяца для месяца, первый день года для года)
    /// </summary>
    public DateTime Period { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; }

    /// <summary>
    /// Название услуги
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;

    /// <summary>
    /// ID услуги
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// Общая прибыль за период
    /// </summary>
    public decimal TotalEarning { get; set; }
}

/// <summary>
/// Запрос на получение прибыли услуг по периодам
/// </summary>
public class GetServiceEarningByPeriodRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID услуг (опционально)
    /// </summary>
    public List<int>? ServiceIds { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; } = GroupPeriod.Day;
}

/// <summary>
/// Прибыль машины за период
/// </summary>
public class CarEarningByPeriodDto
{
    /// <summary>
    /// Период (дата для дня, первый день месяца для месяца, первый день года для года)
    /// </summary>
    public DateTime Period { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; }

    /// <summary>
    /// Номер машины
    /// </summary>
    public string CarNumber { get; set; } = string.Empty;

    public string CarModel { get; set; } = string.Empty;
    /// <summary>
    /// ID машины
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Общая прибыль за период
    /// </summary>
    public decimal TotalEarning { get; set; }
}

/// <summary>
/// Запрос на получение прибыли машин по периодам
/// </summary>
public class GetCarEarningByPeriodRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID машин (опционально)
    /// </summary>
    public List<int>? CarIds { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; } = GroupPeriod.Day;
}

/// <summary>
/// Прибыль водителя за период
/// </summary>
public class DriverEarningByPeriodDto
{
    /// <summary>
    /// Период (дата для дня, первый день месяца для месяца, первый день года для года)
    /// </summary>
    public DateTime Period { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; }

    /// <summary>
    /// Имя водителя
    /// </summary>
    public string DriverName { get; set; } = string.Empty;

    /// <summary>
    /// ID водителя
    /// </summary>
    public int DriverId { get; set; }

    /// <summary>
    /// Общая прибыль за период
    /// </summary>
    public decimal TotalEarning { get; set; }
}

/// <summary>
/// Запрос на получение прибыли водителей по периодам
/// </summary>
public class GetDriverEarningByPeriodRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID водителей (опционально)
    /// </summary>
    public List<int>? DriverIds { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; } = GroupPeriod.Day;
}

/// <summary>
/// Прибыль диспетчера за период
/// </summary>
public class DispatcherEarningByPeriodDto
{
    /// <summary>
    /// Период (дата для дня, первый день месяца для месяца, первый день года для года)
    /// </summary>
    public DateTime Period { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; }

    /// <summary>
    /// Имя диспетчера
    /// </summary>
    public string DispatcherName { get; set; } = string.Empty;

    /// <summary>
    /// ID диспетчера
    /// </summary>
    public int DispatcherId { get; set; }

    /// <summary>
    /// Общая прибыль за период
    /// </summary>
    public decimal TotalEarning { get; set; }
}

/// <summary>
/// Запрос на получение прибыли диспетчеров по периодам
/// </summary>
public class GetDispatcherEarningByPeriodRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Фильтр по ID диспетчеров (опционально)
    /// </summary>
    public List<int>? DispatcherIds { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; } = GroupPeriod.Day;
}

/// <summary>
/// Прибыль компании ZasNet за период
/// </summary>
public class ZasNetEarningByPeriodDto
{
    /// <summary>
    /// Период (дата для дня, первый день месяца для месяца, первый день года для года)
    /// </summary>
    public DateTime Period { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; }

    /// <summary>
    /// Общая прибыль за период (OrderPriceAmount)
    /// </summary>
    public decimal CommonEarning { get; set; }

    /// <summary>
    /// Прибыль Алмаза за период (OrderPriceAmount)
    /// </summary>
    public decimal AlmazEarning { get; set; }

    /// <summary>
    /// Общая прибыль c учетом налога период (OrderPriceAmount)
    /// </summary>
    public decimal CommonEarningWithVat { get; set; }

    /// <summary>
    /// Прибыль Алмаза c учетом налога за период (OrderPriceAmount)
    /// </summary>
    public decimal AlmazEarningWithVat { get; set; }
}

/// <summary>
/// Запрос на получение прибыли компании по периодам
/// </summary>
public class GetZasNetEarningRequest
{
    /// <summary>
    /// Дата начала периода (обязательная)
    /// </summary>
    public DateTime DateFrom { get; set; }

    /// <summary>
    /// Дата окончания периода (обязательная)
    /// </summary>
    public DateTime DateTo { get; set; }

    /// <summary>
    /// Тип периода для группировки
    /// </summary>
    public GroupPeriod GroupPeriod { get; set; } = GroupPeriod.Day;
}


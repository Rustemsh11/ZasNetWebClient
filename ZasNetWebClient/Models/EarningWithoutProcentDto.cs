namespace ZasNetWebClient.Models;

/// <summary>
/// DTO для зарплаты без заявки
/// </summary>
public class EarningWithoutProcentDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Запрос на получение начислений без процентов с фильтрацией
/// </summary>
public class GetEarningWithoutProcentByFilterRequest
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
    /// Список ID сотрудников для фильтрации
    /// </summary>
    public List<int>? EmployeeIds { get; set; }
}

/// <summary>
/// Команда для создания зарплаты без заявки
/// </summary>
public class CreateEarningWithoutProcentCommand
{
    public int EmployeeId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Команда для обновления зарплаты без заявки
/// </summary>
public class UpdateEarningWithoutProcentCommand
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}


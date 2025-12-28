namespace ZasNetWebClient.Models;

/// <summary>
/// Запрос на скачивание отчета по заработку сотрудника
/// </summary>
public class DownloadEmployeeEarningReportRequest
{
    public List<EmployeeEarningByFilterDto> Data { get; set; } = new();
}

/// <summary>
/// Команда для отправки отчета по заработку сотрудника в Telegram
/// </summary>
public class SendEmployeeEarningReportToTelegramCommand
{
    public List<EmployeeEarningByFilterDto> Data { get; set; } = new();
}


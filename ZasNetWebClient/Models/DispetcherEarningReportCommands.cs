namespace ZasNetWebClient.Models;

/// <summary>
/// Запрос на скачивание отчета по заработку диспетчера
/// </summary>
public class DownloadDispetcherEarningReportRequest
{
    public List<DispetcherEarningByFilterDto> Data { get; set; } = new();
}

/// <summary>
/// Команда для отправки отчета по заработку диспетчера в Telegram
/// </summary>
public class SendDispetcherEarningReportToTelegramCommand
{
    public List<DispetcherEarningByFilterDto> Data { get; set; } = new();
}


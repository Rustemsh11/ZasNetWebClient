namespace ZasNetWebClient.Models;

public class GetLockedOrdersResponse
{
    public int Id { get; set; }
    public string OrderClientName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string? LockedByUserName { get; set; }
    public DateTime? LockedAt { get; set; }
}

public class ResetLocksCommand
{
    public int OrderId { get; set; }
}


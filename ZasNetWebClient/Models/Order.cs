namespace ZasNetWebClient.Models;

public class Order
{
    public int Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public string Employee { get; set; }
    public OrderStatus Status { get; set; }
    public List<string> ServiceNames { get; set; }
}

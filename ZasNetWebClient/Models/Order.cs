namespace ZasNetWebClient.Models;

public class Order
{
    public int Id { get; set; }
    public string Client { get; set; } = string.Empty;
    public DateTime DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string Address { get; set; }
    public OrderStatus Status { get; set; }
    public List<int> CarIds { get; set; } = new();
}

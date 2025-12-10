namespace ZasNetWebClient.Models
{
    public record ChangeOrderStatusCommand(int OrderId, OrderStatus OrderStatus);
}

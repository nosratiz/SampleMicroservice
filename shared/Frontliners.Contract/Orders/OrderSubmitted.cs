namespace Frontliners.Contract.Orders;

public sealed record OrderSubmitted(List<OrderItemSubmitted> OrderItems, Guid UserId, Guid OrderId); 

public sealed record OrderItemSubmitted
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
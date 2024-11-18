namespace Frontliners.Order.Applications.Basket.Dto;

public sealed record OrderItemListDto(Guid ProductId, string ProductName, decimal Price, int Quantity, decimal TotalPrice);
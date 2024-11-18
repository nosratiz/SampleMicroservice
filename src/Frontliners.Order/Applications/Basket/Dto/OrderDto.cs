using Frontliners.Order.Domain.Enum;

namespace Frontliners.Order.Applications.Basket.Dto;

public record OrderDto(Guid OrderId, Guid UserId,Status Status, List<OrderItemListDto> OrderItems, decimal TotalPrice);
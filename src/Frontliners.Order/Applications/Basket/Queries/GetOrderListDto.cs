using Frontliners.Order.Applications.Basket.Dto;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Queries;

public sealed record GetOrderListQuery : IRequest<List<OrderItemListDto>>;
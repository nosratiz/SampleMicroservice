using FluentResults;
using Frontliners.Order.Applications.Basket.Dto;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Queries;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<Result<OrderDto>>;
using FluentResults;
using Frontliners.Order.Domain.Entities;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Create;

public sealed record CreateBasketCommand(Guid ProductId, int Quantity) : IRequest<Result<List<OrderItem>>>;
using FluentResults;
using Frontliners.Order.Domain.Entities;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Update;

public sealed record UpdateBasketCommand(Guid ProductId, int Quantity) : IRequest<Result<List<OrderItem>>>;
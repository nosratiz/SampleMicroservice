using FluentResults;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Delete;

public sealed record DeleteBasketCommand(Guid ProductId) : IRequest<Result>;
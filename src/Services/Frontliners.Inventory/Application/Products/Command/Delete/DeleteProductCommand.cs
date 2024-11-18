using FluentResults;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Delete;

public sealed record DeleteProductCommand(Guid ProductId) : IRequest<Result>;
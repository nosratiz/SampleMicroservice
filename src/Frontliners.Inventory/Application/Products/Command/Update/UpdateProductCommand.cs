using FluentResults;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Update;

public sealed record UpdateProductCommand: IRequest<Result>
{
    public Guid ProductId { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
    public int Stock { get; init; }
} 
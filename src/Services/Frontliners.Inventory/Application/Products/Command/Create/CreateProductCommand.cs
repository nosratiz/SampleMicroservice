using FluentResults;
using Frontliners.Inventory.Application.Products.Dto;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Create;

public record CreateProductCommand(string Name, string Description, decimal Price, int Stock) : IRequest<Result<ProductDto>>;


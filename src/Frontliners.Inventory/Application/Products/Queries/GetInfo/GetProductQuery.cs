using FluentResults;
using Frontliners.Inventory.Application.Products.Dto;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Queries;

public sealed record GetProductQuery(Guid ProductId) : IRequest<Result<ProductDto>>;
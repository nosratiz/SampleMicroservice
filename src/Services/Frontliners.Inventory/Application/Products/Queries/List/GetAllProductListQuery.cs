using Frontliners.Inventory.Application.Products.Dto;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Queries.List;

public sealed record GetAllProductListQuery() : IRequest<IEnumerable<ProductListDto>>;
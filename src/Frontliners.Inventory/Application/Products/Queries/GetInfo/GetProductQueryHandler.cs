using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Inventory.Application.Products.Dto;
using Frontliners.Inventory.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Queries;

public sealed class GetProductQueryHandler(IMongoRepository<Product> productRepository, IMapper mapper)
    : IRequestHandler<GetProductQuery, Result<ProductDto>>
{

    public async Task<Result<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductId, cancellationToken);

        if (product is null)
        {
            return Result.Fail($"Product with id {request.ProductId} not found");
        }

        var productDto = mapper.Map<ProductDto>(product);

        return Result.Ok(productDto);
    }
}
using Frontliners.Common.Mongo.Repository;
using Frontliners.Inventory.Application.Products.Dto;
using Frontliners.Inventory.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Queries.List;

public sealed class GetAllProductListQueryHandler(IMongoRepository<Product> productRepository,IMapper mapper)
    : IRequestHandler<GetAllProductListQuery, IEnumerable<ProductListDto>>
{

    public async Task<IEnumerable<ProductListDto>> Handle(GetAllProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.FindAsync(x=>x.IsDeleted==false,cancellationToken);

        var productListDto = mapper.Map<IEnumerable<ProductListDto>>(products);
        
        return productListDto;
    }
}
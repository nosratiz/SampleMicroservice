using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Inventory.Application.Products.Dto;
using Frontliners.Inventory.Domain.Entities;
using MapsterMapper;
using MassTransit;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Create;

public sealed class CreateProductCommandHandler(IMongoRepository<Product> productRepository,
    IMapper mapper,
    IPublishEndpoint publishEndpoint,
    ILogger<CreateProductCommandHandler> logger)
    : IRequestHandler<CreateProductCommand,Result<ProductDto>>
{

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var newProduct= new Product(request.Name, request.Description, request.Price, request.Stock);

        await productRepository.AddAsync(newProduct, cancellationToken);
        
        var productCreated= mapper.Map<ProductCreated>(newProduct);
        
        await publishEndpoint.Publish(productCreated, cancellationToken);
        
        logger.LogInformation($"Product with id {newProduct.Id} created");
        
        return Result.Ok(mapper.Map<ProductDto>(newProduct));
        
    }
}
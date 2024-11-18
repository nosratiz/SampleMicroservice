using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Inventory.Domain.Entities;
using Frontliners.Inventory.InfraStructure;
using MapsterMapper;
using MassTransit;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Update;

public sealed class UpdateProductCommandHandler(
    IMongoRepository<Product> productRepository,
    IMapper mapper,
    ILogger<UpdateProductCommandHandler> logger,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<UpdateProductCommand, Result>
{


    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductId, cancellationToken);
       
        if (product is null)
        {
            return Result.Fail(InventoryErrorMessage.ProductNotFound);
        }

        product.Update(request.Name, request.Description, request.Price, request.Stock);

        await productRepository.UpdateAsync(product, cancellationToken);
        
        var productUpdated = mapper.Map<ProductUpdated>(product);
        
        await publishEndpoint.Publish(productUpdated, cancellationToken);
        
        logger.LogInformation($"Product with id {product.Id} updated");
        
        return Result.Ok();
    }
}
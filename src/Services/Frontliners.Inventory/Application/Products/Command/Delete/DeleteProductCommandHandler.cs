using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Inventory.Domain.Entities;
using MassTransit;
using MediatR;

namespace Frontliners.Inventory.Application.Products.Command.Delete;

public sealed class DeleteProductCommandHandler(
    IMongoRepository<Product> productRepository,
    IPublishEndpoint publishEndpoint,ILogger<DeleteProductCommandHandler> logger)
    : IRequestHandler<DeleteProductCommand,Result>
{
   

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetAsync(request.ProductId, cancellationToken);

        if (product is null)
            return Result.Fail($"Product with id {request.ProductId} not found");
        
        product.Delete();

        await productRepository.UpdateAsync(product, cancellationToken);

        await publishEndpoint.Publish(new ProductDeleted{ProductId = request.ProductId} , cancellationToken);
        
        logger.LogInformation($"Product with id {request.ProductId} deleted");
        
        return Result.Ok();
    }
}
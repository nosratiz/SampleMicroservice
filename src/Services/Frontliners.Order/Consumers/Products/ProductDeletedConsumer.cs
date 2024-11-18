using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Order.Domain.Entities;
using MassTransit;

namespace Frontliners.Order.Consumers.Products;

public sealed class ProductDeletedConsumer(
    IMongoRepository<Product> productRepository,
    ILogger<ProductDeletedConsumer> logger)
    : IConsumer<ProductDeleted>
{
    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId, context.CancellationToken);

        if (product is null)
        {
            logger.LogWarning($"Product with id {context.Message.ProductId} not found");
            return;
        }

        product.Delete();

        await productRepository.UpdateAsync(product, context.CancellationToken);

        logger.LogInformation($"Product with id {product.Id} deleted");
    }
}
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Order.Domain.Entities;
using MassTransit;

namespace Frontliners.Order.Consumers.Products;

public sealed class ProductUpdatedConsumer(
    IMongoRepository<Product> productRepository,
    ILogger<ProductUpdatedConsumer> logger)
    : IConsumer<ProductUpdated>
{
    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId, context.CancellationToken);

        if (product is null)
        {
            logger.LogWarning($"Product with id {context.Message.ProductId} not found");
            return;
        }

        product.Update(context.Message.Name, context.Message.Price, context.Message.Stock);

        await productRepository.UpdateAsync(product, context.CancellationToken);

        logger.LogInformation($"Product with id {product.Id} updated");
    }
}
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Orders;
using Frontliners.Inventory.Domain.Entities;
using MassTransit;

namespace Frontliners.Inventory.Consumers;

public sealed class OrderSubmittedConsumer(
    IMongoRepository<Product> productRepository,
    ILogger<OrderSubmittedConsumer> logger)
    : IConsumer<OrderSubmitted>
{
    public async Task Consume(ConsumeContext<OrderSubmitted> context)
    {
        var products = await productRepository
            .FindAsync(x=> context.Message.OrderItems
                .Select(e=>e.ProductId)
                .Contains(x.Id), context.CancellationToken);

        foreach (var product in products)
        {
            var orderItem = context.Message.OrderItems
                .First(x => x.ProductId == product.Id);
            
            product.Sell(orderItem.Quantity);
            
            await productRepository.UpdateAsync(product, context.CancellationToken);
        }
        
        logger.LogInformation($"Order with id {context.Message.OrderId} submitted");
    }
}
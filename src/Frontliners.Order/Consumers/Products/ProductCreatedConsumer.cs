using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Products;
using Frontliners.Order.Domain.Entities;
using MassTransit;
using MongoDB.Driver;

namespace Frontliners.Order.Consumers.Products;

public sealed class ProductCreatedConsumer(
    IMongoRepository<Product> productRepository,
    ILogger<ProductCreatedConsumer> logger)
    : IConsumer<ProductCreated>
{
    
    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var product = new Product(context.Message.ProductId, context.Message.Name, context.Message.Price, context.Message.Stock);
        try
        {
            await productRepository.AddAsync(product, context.CancellationToken);
        
            logger.LogInformation($"Product with id {product.Id} created");
        }
        
        catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
        {
            logger.LogError($"Product with id {context.Message.ProductId} already exists.");
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Error while creating Product with id {context.Message.ProductId}");
            throw;
        }  
      
    }
}
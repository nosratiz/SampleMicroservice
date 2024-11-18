using FluentResults;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Contract.Orders;
using Frontliners.Order.Domain.Entities;
using Frontliners.Order.Domain.Enum;
using Frontliners.Order.InfraStructures;
using MassTransit;
using MediatR;

namespace Frontliners.Order.Applications.Orders.Command.Submit;

public class SubmitCommandHandler(
    IMongoRepository<Domain.Entities.Order> orderRepository,
    IMongoRepository<Product> productRepository,
    ICurrentUserService currentUserService,ILogger<SubmitCommandHandler> logger,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<SubmitCommand, Result>
{
   
    public async Task<Result> Handle(SubmitCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(x =>
            x.UserId == Guid.Parse(currentUserService.UserId!) &&
            x.Status == Status.InBasket, cancellationToken);
        
        if (order is null)
            return Result.Fail(OrderErrorMessage.OrderNotFound);
        
        order.Checkout();
        
        var productIds = order.Items.Select(x => x.ProductId).ToList();
        
        var products = await productRepository.FindAsync(x => productIds.Contains(x.Id), cancellationToken);
        
        foreach (var product in products)
        {
            var orderItem = order.Items.First(x => x.ProductId == product.Id);
            product.Sell(orderItem.Quantity);
            
            await productRepository.UpdateAsync(product, cancellationToken);
        }
        
        await orderRepository.UpdateAsync(order, cancellationToken);
        
        await publishEndpoint.Publish(new 
                OrderSubmitted(order.Items.Select(x => new OrderItemSubmitted
                    {ProductId = x.ProductId,
                        Quantity = x.Quantity
                    }).ToList()
                   ,order.UserId,order.Id),
            cancellationToken);
        
        return Result.Ok();
    }
}
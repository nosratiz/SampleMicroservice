using FluentResults;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Domain.Entities;
using Frontliners.Order.Domain.Enum;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Create;

public sealed class CreateBasketCommandHandler(
    IMongoRepository<Domain.Entities.Order> orderRepository,IMongoRepository<Product> productRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<CreateBasketCommand, Result<List<OrderItem>>>
{
    public async Task<Result<List<OrderItem>>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(currentUserService.UserId!);
        var order = await orderRepository.GetAsync(x =>
            x.UserId ==  userId &&
            x.Status == Status.InBasket, cancellationToken);


        if (order is null)
        {
            order = new Domain.Entities.Order();
            order.AddOrder(userId);
            
            await orderRepository.AddAsync(order, cancellationToken);
        }
        
        var orderItemExist= order.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        
        if (orderItemExist is not null)
        {
            orderItemExist.UpdateQuantity(request.Quantity);
        }
        else
        {
            var product = await productRepository.GetAsync(request.ProductId, cancellationToken);
        
            order.AddItem(request.ProductId,request.Quantity,product!.Price,product.Name);
        }
        
        await orderRepository.UpdateAsync(order, cancellationToken);
        
        return Result.Ok(order.Items.ToList());
    }
}
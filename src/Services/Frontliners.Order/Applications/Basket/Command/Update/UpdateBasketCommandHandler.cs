using FluentResults;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Domain.Entities;
using Frontliners.Order.Domain.Enum;
using Frontliners.Order.InfraStructures;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Update;

public sealed class UpdateBasketCommandHandler(IMongoRepository<Domain.Entities.Order> orderRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateBasketCommand, Result<List<OrderItem>>>
{

    public async Task<Result<List<OrderItem>>> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(x =>
            x.UserId == Guid.Parse(currentUserService.UserId!) &&
            x.Status == Status.InBasket, cancellationToken);

        if (order is null)
            return Result.Fail(OrderErrorMessage.OrderNotFound);
        
        var orderItemExist= order.Items.FirstOrDefault(x => x.ProductId == request.ProductId);
        
        if (orderItemExist is null)
            return Result.Fail(OrderErrorMessage.OrderItemNotFound);
        
        order.UpdateItem(request.ProductId,request.Quantity);
        
        await orderRepository.UpdateAsync(order, cancellationToken);
        
        return Result.Ok(order.Items.ToList());
    }
}
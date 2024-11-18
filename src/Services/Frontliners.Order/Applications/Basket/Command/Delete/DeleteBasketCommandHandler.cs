using FluentResults;
using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Domain.Enum;
using Frontliners.Order.InfraStructures;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Command.Delete;

public sealed class DeleteBasketCommandHandler(
    IMongoRepository<Domain.Entities.Order> orderRepository,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteBasketCommand, Result>
{
 

    public async Task<Result> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(x =>
            x.UserId == Guid.Parse(currentUserService.UserId!) &&
            x.Status == Status.InBasket, cancellationToken);

        if (order is null)
            return Result.Fail(OrderErrorMessage.OrderNotFound);
        
        order.RemoveItem(request.ProductId);
        
        await orderRepository.UpdateAsync(order, cancellationToken);
        
        return Result.Ok();
    }
}
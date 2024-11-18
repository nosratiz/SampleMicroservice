using FluentResults;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Applications.Basket.Dto;
using MapsterMapper;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Queries;

public sealed class GetOrderQueryHandler(IMongoRepository<Domain.Entities.Order> orderRepository, IMapper mapper)
    : IRequestHandler<GetOrderQuery, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(request.OrderId, cancellationToken);
        
        if (order is null)
        {
            return Result.Fail($"Order with id {request.OrderId} not found");
        }
        
        var orderDto = mapper.Map<OrderDto>(order);
        
        return Result.Ok(orderDto);
    }
}
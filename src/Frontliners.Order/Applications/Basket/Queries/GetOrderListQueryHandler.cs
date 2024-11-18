using Frontliners.Common.InfraStructure.Claims;
using Frontliners.Common.Mongo.Repository;
using Frontliners.Order.Applications.Basket.Dto;
using Frontliners.Order.Domain.Enum;
using MapsterMapper;
using MediatR;

namespace Frontliners.Order.Applications.Basket.Queries;

public sealed class GetOrderListQueryHandler(
    IMongoRepository<Domain.Entities.Order> orderRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetOrderListQuery, List<OrderItemListDto>>
{
    

    public async Task<List<OrderItemListDto>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetAsync(x =>
            x.UserId == Guid.Parse(currentUserService.UserId!) &&
            x.Status == Status.InBasket, cancellationToken);
        
        if (order is null) return new List<OrderItemListDto>();
        
        var orderItemListDto = mapper.Map<List<OrderItemListDto>>(order.Items);
        
        return orderItemListDto;
    }
}
using Frontliners.Order.Applications.Basket.Command.Create;
using Frontliners.Order.Applications.Basket.Command.Delete;
using Frontliners.Order.Applications.Basket.Command.Update;
using Frontliners.Order.Applications.Basket.Queries;
using MediatR;

namespace Frontliners.Order.EndPoints;

public static class BasketEndPoints
{
    public static void MapBasketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/baskets", async (IMediator mediator,CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new GetOrderListQuery(),cancellationToken);
            return Results.Ok(result);
        }).RequireAuthorization();

        app.MapPost("/baskets", async (IMediator mediator, CreateBasketCommand dto,CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(dto,cancellationToken);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).RequireAuthorization();

        app.MapDelete("/baskets/{productId}", async (IMediator mediator, Guid productId,CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DeleteBasketCommand(productId),cancellationToken);
          
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).RequireAuthorization();

        app.MapPut("/baskets", async (IMediator mediator,UpdateBasketCommand command,CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(command,cancellationToken);
            
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).RequireAuthorization();
    }
}
using Frontliners.Order.Applications.Basket.Queries;
using Frontliners.Order.Applications.Orders.Command.Submit;
using MediatR;

namespace Frontliners.Order.EndPoints;

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/checkout", async (IMediator mediator, SubmitCommand command) =>
        {
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.NoContent(): Results.BadRequest();
        }).RequireAuthorization();

        app.MapGet("/checkout/{id}", async (IMediator mediator, Guid id) =>
        {
            var result = await mediator.Send(new GetOrderQuery(id));
            return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound();
        }).RequireAuthorization();

      
    }
}
using Frontliners.Identity.Application.Auth.Command.Login;
using Frontliners.Identity.Application.Auth.Command.Register;
using Frontliners.Identity.Application.Auth.Dto;
using MediatR;

namespace Frontliners.Identity.Endpoints;

public static class AuthEndPoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (IMediator mediator, LoginCommand command) =>
        {
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Errors);
        }).Produces<LoginResponse>()
            .Produces(400)
            .WithSummary("Bad Request");
        
        app.MapPost("/register", async (IMediator mediator, RegisterCommand command) =>
        {
            var result = await mediator.Send(command);
            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
        }).Produces(200);
    }
}
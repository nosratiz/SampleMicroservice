using Frontliners.Inventory.Application.Products.Command.Create;
using Frontliners.Inventory.Application.Products.Command.Delete;
using Frontliners.Inventory.Application.Products.Command.Update;
using Frontliners.Inventory.Application.Products.Queries;
using Frontliners.Inventory.Application.Products.Queries.List;
using MediatR;


namespace Frontliners.Inventory.EndPoints;

public static class ProductEndPoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (ISender sender,CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetAllProductListQuery(), cancellationToken);
           
            return Results.Ok(result);
            
        });

        app.MapGet("/api/products/{id}", async (ISender sender, Guid id,CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetProductQuery(id),cancellationToken);
            
            return result.IsSuccess is false ? Results.NotFound(result.Errors) : Results.Ok(result);
        });

        app.MapPost("/api/products", async (IMediator mediator, CreateProductCommand command,CancellationToken cancellationToken) =>
        {
           var result= await mediator.Send(command, cancellationToken);
            return Results.Created($"/api/products/{result.Value.ProductId}", result.Value);
        });

        app.MapPut("/api/products/{id}", async (IMediator mediator, Guid id, UpdateProductCommand command) =>
        {
            command.ProductId=id;
          
            var result = await mediator.Send(command);
            
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Errors);
        });

        app.MapDelete("/api/products/{id}", async (IMediator mediator, Guid id) =>
        {
            await mediator.Send(new DeleteProductCommand(id));
          
            return Results.NoContent();
        });
    }
    
}
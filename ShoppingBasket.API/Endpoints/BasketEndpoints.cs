using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Aplication.Features.Baskets.CloseBasket;
using ShoppingBasket.Aplication.Features.Baskets.CreateBasket;
using ShoppingBasket.Aplication.Features.Baskets.CreateItem;
using ShoppingBasket.Aplication.Features.Baskets.DeleteItem;
using ShoppingBasket.Aplication.Features.Baskets.GetBasketByBasketId;
using ShoppingBasket.Aplication.Features.Baskets.GetClosedBasketsByCostumerId;
using ShoppingBasket.Aplication.Features.Baskets.GetOpenBasketByCustomerId;
using ShoppingBasket.Aplication.Features.Discounts;

namespace ShoppingBasket.API.Endpoints;

public class BasketEndpoints :IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/baskets")
            .WithTags("baskets");

        group.MapPost("/", async (IMediator mediator, [AsParameters] CreateBasketCommand command) =>
        {
            if(command.CustomerId is null && command.GuestId is null)
            {
                return Results.BadRequest("Either UserId or BasketId must be provided.");
            }
            var basketId = await mediator.Send(command);
            var response = await mediator.Send(new GetBasketByBasketIdRequest(basketId));
            return Results.Ok(response);
        });
        
        group.MapGet("/{basketId}", async (IMediator mediator, Guid basketId) =>
        {
            var response = await mediator.Send(new GetBasketByBasketIdRequest(basketId));
            return Results.Ok(response);
        });
        
        group.MapPost("/{basketId}/product", async (IMediator mediator, Guid basketId, [FromBody] CreateItemCommand command) =>
        {
            await mediator.Send(command with { BasketId = basketId });
            var response = await mediator.Send(new GetBasketByBasketIdRequest(basketId));
            return Results.Ok(response);
        });
        
        group.MapDelete("/{basketId}/product/{productId}", async (IMediator mediator, Guid basketId, Guid productId) =>
        {
            await mediator.Send(new DeleteItemCommand(basketId, productId));
            var response = await mediator.Send(new GetBasketByBasketIdRequest(basketId));
            return Results.Ok(response);
        });
        
        group.MapPatch("/{basketId}/close", async (IMediator mediator, Guid basketId) =>
        {
            await mediator.Send(new CloseBasketCommand(basketId));
            return Results.NoContent();
        });
        
        group.MapGet("/customer/{customerId}", async (IMediator mediator, Guid customerId) =>
        {
            var response = await mediator.Send(new GetOpenBasketByCustomerIdRequest(customerId));
            return Results.Ok(response);
        });
        
        group.MapGet("/customer/{customerId}/history", async (IMediator mediator, Guid customerId, 
            [AsParameters]GetClosedBasketsByCustomerIdRequest request) =>
        {
            var response = await mediator.Send(request with { CustomerId = customerId });
            return Results.Ok(response);
        });
    }
}
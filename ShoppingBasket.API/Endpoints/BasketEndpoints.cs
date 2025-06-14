using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.Aplication.Features.Baskets.CancelBasket;
using ShoppingBasket.Aplication.Features.Baskets.CloseBasket;
using ShoppingBasket.Aplication.Features.Baskets.CreateBasket;
using ShoppingBasket.Aplication.Features.Baskets.CreateItem;
using ShoppingBasket.Aplication.Features.Baskets.DeleteItem;
using ShoppingBasket.Aplication.Features.Baskets.GetBasketByBasketId;
using ShoppingBasket.Aplication.Features.Baskets.GetClosedBasketsByCostumerId;
using ShoppingBasket.Aplication.Features.Baskets.GetOpenBasketByCustomerId;
using ISession = ShoppingBasket.Domain.Auth.ISession;

namespace ShoppingBasket.API.Endpoints;

public class BasketEndpoints :IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/baskets")
            .WithTags("baskets");

        group.MapPost("/", async (IMediator mediator, ISession session, [FromBody] CreateBasketCommand command) =>
        {
   
            if(session.UserId is null && command.GuestId is null)
            {
                return Results.BadRequest("Either UserId or BasketId must be provided.");
            }
            var basketId = await mediator.Send(command with{CustomerId = session.UserId});
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
        
        group.MapDelete("/{basketId}/product/{productId}", async (IMediator mediator, Guid basketId, Guid productId, [FromQuery]Guid? discountId = null) =>
        {
            await mediator.Send(new DeleteItemCommand(basketId, productId,discountId));
            var response = await mediator.Send(new GetBasketByBasketIdRequest(basketId));
            return Results.Ok(response);
        });
        
        group.MapPatch("/{basketId}/close", async (IMediator mediator, Guid basketId) =>
        {
            await mediator.Send(new CloseBasketCommand(basketId));
            return Results.NoContent();
        });
        
        group.MapPatch("/{basketId}/cancel", async (IMediator mediator, Guid basketId) =>
        {
            await mediator.Send(new CancelBasketCommand(basketId));
            return Results.NoContent();
        });

        group.MapGet("/customer/current", async (IMediator mediator, ISession session) =>
        {
            if (session.UserId is null)
            {
                return Results.BadRequest("UserId must be provided.");
            }

            var response = await mediator.Send(new GetOpenBasketByCustomerIdRequest(session.UserId.Value));
            return Results.Ok(response);
        });

        group.MapGet("/customer/history", async (IMediator mediator, ISession session,
            [AsParameters] GetClosedBasketsByCustomerIdRequest request) =>
        {
            if (session.UserId is null)
            {
                return Results.BadRequest("UserId must be provided.");
            }

            var response = await mediator.Send(request with { CustomerId = session.UserId.Value });
            return Results.Ok(response);
        });
    }
}
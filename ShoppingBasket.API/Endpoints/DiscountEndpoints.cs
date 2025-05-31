using MediatR;
using ShoppingBasket.Aplication.Features.Discounts;
using ShoppingBasket.Aplication.Features.Products.GetAllProducts;

namespace ShoppingBasket.API.Endpoints;

public class DiscountEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/discounts")
            .WithTags("discounts");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var discounts = await mediator.Send(new GetAllDiscountsRequest());
            return Results.Ok(discounts);
        });
    }
}
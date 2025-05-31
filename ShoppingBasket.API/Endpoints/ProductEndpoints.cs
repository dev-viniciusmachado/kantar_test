using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Aplication.Features.Products.GetAllProducts;

namespace ShoppingBasket.API.Endpoints;

public class ProductEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        
        var group = app.MapGroup("api/products")
            .WithTags("products");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var products = await mediator.Send(new GetAllProductsRequest());
            return Results.Ok(products);
        });
    }
}
using MediatR;

namespace ShoppingBasket.Aplication.Features.Products.GetAllProducts;

public record GetAllProductsRequest() : IRequest<IEnumerable<ProductResponse>>;
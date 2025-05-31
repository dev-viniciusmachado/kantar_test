using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.GetOpenBasketByCustomerId;

public record GetOpenBasketByCustomerIdRequest(Guid CustomerId) : IRequest<BasketResponse?>;
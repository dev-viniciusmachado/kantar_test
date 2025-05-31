using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.GetBasketByBasketId;

public record GetBasketByBasketIdRequest(Guid BasketId) : IRequest<BasketResponse>;
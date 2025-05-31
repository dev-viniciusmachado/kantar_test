using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateBasket;

public record CreateBasketCommand(Guid? CustomerId, Guid? GuestId) : IRequest<Guid>;
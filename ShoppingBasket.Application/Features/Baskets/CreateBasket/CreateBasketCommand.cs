using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateBasket;

public record CreateBasketCommand(Guid ProductId, int Quantity,Guid? CustomerId, Guid? GuestId) : IRequest<Guid>;
using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.CloseBasket;

public record CloseBasketCommand(Guid BasketId) : IRequest;
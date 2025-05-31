using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.DeleteItem;

public record DeleteItemCommand(Guid BasketId, Guid ProductId) : IRequest<Guid>;
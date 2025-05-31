using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.CreateItem;

public record CreateItemCommand(Guid BasketId, Guid ProductId, int Quantity) : IRequest<Guid>;

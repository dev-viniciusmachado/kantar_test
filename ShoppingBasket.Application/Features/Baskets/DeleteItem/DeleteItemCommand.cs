using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.DeleteItem;

public record DeleteItemCommand(Guid BasketId, Guid ProductId, Guid? DiscountId) : IRequest<Guid>;
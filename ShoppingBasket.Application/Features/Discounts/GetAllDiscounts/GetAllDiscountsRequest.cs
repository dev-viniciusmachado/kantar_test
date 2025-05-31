using MediatR;

namespace ShoppingBasket.Aplication.Features.Discounts;

public record GetAllDiscountsRequest() : IRequest<IEnumerable<DiscountResponse>>;
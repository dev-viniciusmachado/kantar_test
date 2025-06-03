using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.GetClosedBasketsByCostumerId;

public record GetClosedBasketsByCustomerIdRequest(Guid? CustomerId, string OrderBy = "ClosedAt", bool IsDescending = true, int Page = 0, int PageSize = 10) : IRequest<PagedBasketResponse>;
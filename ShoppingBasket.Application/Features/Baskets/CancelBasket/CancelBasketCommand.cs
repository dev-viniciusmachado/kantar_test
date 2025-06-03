using System;
using MediatR;

namespace ShoppingBasket.Aplication.Features.Baskets.CancelBasket;

public record CancelBasketCommand(Guid BasketId) : IRequest;
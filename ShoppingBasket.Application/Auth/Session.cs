

using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShoppingBasket.Domain.Auth;

namespace ShoppingBasket.Aplication.Auth;

public class Session : ISession
{
    public Guid? UserId { get; private init; }

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        var nameIdentifier = user?.FindFirst(ClaimTypes.NameIdentifier);

        if(nameIdentifier != null)
        {
            UserId = new Guid(nameIdentifier.Value);
        }
    }

}
using ShoppingBasket.Domain.Auth;

namespace ShoppingBasket.Tests.Integration.helper;

public class FakeSession : ISession
{
    public Guid? UserId
    {
        get
        {
            return Guid.Parse("00000000-0000-0000-0000-000000000001");
        }
    }
}
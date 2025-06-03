namespace ShoppingBasket.Domain.Auth;

public interface ISession
{
    public Guid? UserId { get; }
}
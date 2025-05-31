using Microsoft.AspNetCore.Identity;
namespace ShoppingBasket.Infrastructure;

public class ApplicationUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
}

public class ApplicationRole : IdentityRole<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();
}
using ShoppingBasket.Aplication.Auth;
using ISession = ShoppingBasket.Domain.Auth.ISession;

namespace ShoppingBasket.API.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        services.AddScoped<ISession,Session>();

        return services;
    }
}
using ShoppingBasket.Domain.Services.DiscountPolicy;

namespace ShoppingBasket.API.Configurations;

public static class DomainServicesSetup
{
    public static IServiceCollection AddDomaimServicesSetup(this IServiceCollection services)
    {
        services.AddSingleton<IDiscountPolicy, DirecltyDiscountPolicy>();
        services.AddSingleton<IDiscountPolicy, MultiBuyDiscountPolicy>();

        return services;
    }
}
using ShoppingBasket.Aplication;

namespace ShoppingBasket.API.Configurations;

public static class MediatRSetup
{
    public static IServiceCollection AddMediatRSetup(this IServiceCollection services)
    {
        services.AddMediatR((config) =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(IAssemblyMarker));
        });
        
        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Infrastructure;
using ShoppingBasket.Infrastructure.Cache;

namespace ShoppingBasket.API.Configurations;

public static class PersistenceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetConnectionString("SBDb") != null)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("SBDb"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("SBDb")),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure());
            });
            services.AddHostedService<ApplicationDbInitializer>();
            services.AddHostedService<ApplicationDbSeed>();
        }

        services.AddScoped<IContext, ApplicationDbContext>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }
}
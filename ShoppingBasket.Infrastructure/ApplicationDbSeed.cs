using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Entities;
using ShoppingBasket.Domain.Enums;
using ShoppingBasket.Domain.ValueObjects;

namespace ShoppingBasket.Infrastructure;

public class ApplicationDbSeed : IHostedService
{
    private readonly ILogger<ApplicationDbSeed> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbSeed(ILogger<ApplicationDbSeed> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        if (await context.Products.AnyAsync(cancellationToken: cancellationToken)) return;

        var apple = Product.Create("Apple", new Price(1.00m),
            "https://domf5oio6qrcr.cloudfront.net/medialibrary/11525/0a5ae820-7051-4495-bcca-61bf02897472.jpg",
            ProductCategory.Fruit);
        var bread = Product.Create("Bread", new Price(0.65m),
            "https://assets.bonappetit.com/photos/5c62e4a3e81bbf522a9579ce/1:1/w_2254,h_2254,c_limit/milk-bread.jpg",
            ProductCategory.Bakery);
        var soup = Product.Create("Soup", new Price(0.50m),
            "https://s7d5.scene7.com/is/image/CentralMarket/000459562-1?hei=233&wid=233&$large$",
            ProductCategory.PreparedFoods);
        var milk = Product.Create("Milk", new Price(1.30m),
            "https://as1.ftcdn.net/jpg/01/06/68/88/1000_F_106688812_rVoRFXazgIMEUJdvffG9p0XvP8Lntf0a.jpg",
            ProductCategory.Dairy);

        await context.Products.AddRangeAsync([
            apple,
            bread,
            soup,
            milk
        ], cancellationToken);
        
        var discount1 = Discount.CreateDirecltyDiscount(
            "10% off Apples",
            apple.Id,
            10,
            true
        );
        var discount2 = Discount.CreateMultiBuyDiscount(
            "Half Price Bread for every 2 Soups",
            bread.Id,
            50,
            true,
            1,
            soup.Id,
            2
        );
        
        await context.Discounts.AddRangeAsync([
            discount1,
            discount2
        ], cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.CompletedTask;
}
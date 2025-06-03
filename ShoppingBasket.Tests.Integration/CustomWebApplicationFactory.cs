using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Domain.Auth;
using ShoppingBasket.Infrastructure;
using ShoppingBasket.Tests.Integration.helper;
using Testcontainers.MySql;

namespace ShoppingBasket.Tests.Integration;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MySqlContainer _mySqlContainer;

    public CustomWebApplicationFactory()
    {
        _mySqlContainer = new MySqlBuilder()
            .WithDatabase("ShoppingBasketDb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(_mySqlContainer.GetConnectionString(), ServerVersion.AutoDetect(_mySqlContainer.GetConnectionString())));
            services.AddHostedService<ApplicationDbInitializer>();
            services.AddHostedService<ApplicationDbSeed>();
            services.AddScoped<IContext, ApplicationDbContext>();
            services.AddSingleton<ISession, FakeSession>();
        });
    }
}
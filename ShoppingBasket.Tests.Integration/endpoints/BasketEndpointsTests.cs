using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingBasket.Aplication.common;
using ShoppingBasket.Aplication.Features.Baskets;
using ShoppingBasket.Aplication.Features.Baskets.CreateBasket;
using ShoppingBasket.Aplication.Features.Baskets.CreateItem;


namespace ShoppingBasket.Tests.Integration.endpoints;

public class BasketEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string version = "v1";
    private readonly HttpClient _client;
    
    private readonly CustomWebApplicationFactory _factory;

    public BasketEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task CreateBasket_ShouldReturnOk()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();
        
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,Guid.NewGuid());
        // Act
        var response = await _client.PostAsJsonAsync($"/api/{version}/baskets/", createBasketCommand);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await response.Content.ReadFromJsonAsync<BasketResponse>();
        basket.Should().NotBeNull();
        basket!.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetBasketById_ShouldReturnBasket()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();
        
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,Guid.NewGuid());
        var responsePost = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        responsePost.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basketCreated = await responsePost.Content.ReadFromJsonAsync<BasketResponse>();
        // Act
        var response = await _client.GetAsync($"/api/{version}/baskets/{basketCreated.Id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await response.Content.ReadFromJsonAsync<BasketResponse>();
        basket.Should().NotBeNull();
        basket!.Id.Should().Be(basketCreated.Id);
    }
    
    [Fact]
    public async Task CreateItem_ShouldReturnBasketWith2Itens()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product1 = await context.Products.FirstOrDefaultAsync();
        var product2 = await context.Products.FirstOrDefaultAsync(w => w.Id != product1!.Id);
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product1.Id,1,null,Guid.NewGuid());
        var responsePost = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        responsePost.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basketCreated = await responsePost.Content.ReadFromJsonAsync<BasketResponse>();

        var createItem = new CreateItemCommand(basketCreated.Id, product2.Id, 1);
        // Act
        var response = await _client.PostAsJsonAsync($"/api/{version}/baskets/{basketCreated.Id}/product",createItem);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await response.Content.ReadFromJsonAsync<BasketResponse>();
        basket.Should().NotBeNull();
        basket!.Items.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task RemoveItem_ShouldReturnBasketWith1Itens()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product1 = await context.Products.FirstOrDefaultAsync();
        var product2 = await context.Products.FirstOrDefaultAsync(w => w.Id != product1!.Id);
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product1.Id,1,null,Guid.NewGuid());
        var responsePost = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        responsePost.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basketCreated = await responsePost.Content.ReadFromJsonAsync<BasketResponse>();
        var createItem = new CreateItemCommand(basketCreated.Id, product2.Id, 1);

        // Act
        await _client.PostAsJsonAsync($"/api/{version}/baskets/{basketCreated.Id}/product", createItem);
        var response = await _client.DeleteAsync($"/api/{version}/baskets/{basketCreated.Id}/product/{product2.Id}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await response.Content.ReadFromJsonAsync<BasketResponse>();
        basket.Should().NotBeNull();
        basket!.Items.Should().HaveCount(1);
    }
    
    [Fact]
    public async Task CloseBasket_ShouldSetClosedAt()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();
        
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,Guid.NewGuid());
        var basketResponse = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        basketResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await basketResponse.Content.ReadFromJsonAsync<BasketResponse>();
        // Act
        var response = await _client.PatchAsync($"/api/{version}/baskets/{basket.Id}/close",null);

        
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        var basketDb = context.Baskets.Find(basket.Id);
        basketDb.ClosedAt.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CancelBasket_ShouldSetCancelAt()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();
        
        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,Guid.NewGuid());
        var basketResponse = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        basketResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await basketResponse.Content.ReadFromJsonAsync<BasketResponse>();
        // Act
        var response = await _client.PatchAsync($"/api/{version}/baskets/{basket.Id}/cancel",null);
        
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        var basketDb = context.Baskets.Find(basket.Id);
        basketDb.CanceledAt.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetCurrentBasketByUserId_ShouldReturnBasket()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();

        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,null);
        var basketResponse = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        basketResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await basketResponse.Content.ReadFromJsonAsync<BasketResponse>();

        // Act
        var response = await _client.GetAsync($"/api/{version}/baskets/customer/current");
        var basketByUser = await response.Content.ReadFromJsonAsync<BasketResponse>();
        
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        basketByUser.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetHistoryBasketByUserId_ShouldReturnBasket()
    {
        using var scope = _factory.Services.CreateScope();
        IContext context = scope.ServiceProvider.GetRequiredService<IContext>();
        var product = await context.Products.FirstOrDefaultAsync();

        // Arrange
        var createBasketCommand = new CreateBasketCommand(product.Id,1,null,null);
        var basketResponse = await _client.PostAsJsonAsync($"/api/{version}/baskets", createBasketCommand);
        basketResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var basket = await basketResponse.Content.ReadFromJsonAsync<BasketResponse>();
        await _client.PatchAsync($"/api/{version}/baskets/{basket.Id}/close",null);

        // Act
        var response = await _client.GetAsync($"/api/{version}/baskets/customer/history");
        var baskets = await response.Content.ReadFromJsonAsync<PagedBasketResponse>();
        
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        baskets.Baskets.Count().Should().Be(1);
    }
}
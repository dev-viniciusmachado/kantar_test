using ShoppingBasket.Aplication.Features.Products;

namespace ShoppingBasket.Tests.Integration.endpoints;


public class ProductEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    private readonly CustomWebApplicationFactory _factory;

    public ProductEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }
    
    [Fact]
    public async Task GetAllProducts_ShouldReturnProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/products");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
        products.Should().NotBeNull();
        products!.Count.Should().BeGreaterThan(0);
    }
}
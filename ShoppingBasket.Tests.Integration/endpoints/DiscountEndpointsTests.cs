using ShoppingBasket.Aplication.Features.Discounts;

namespace ShoppingBasket.Tests.Integration.endpoints;

public class DiscountEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    private readonly CustomWebApplicationFactory _factory;

    public DiscountEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }
    
    [Fact]
    public async Task GetAllDiscounts_ShouldReturnDiscounts()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/discounts");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var discounts = await response.Content.ReadFromJsonAsync<List<DiscountResponse>>();
        discounts.Should().NotBeNull();
        discounts!.Count.Should().BeGreaterThan(0);
    }
}
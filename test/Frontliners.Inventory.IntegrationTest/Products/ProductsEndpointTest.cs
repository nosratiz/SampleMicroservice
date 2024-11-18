

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Frontliners.Inventory.Application.Products.Command.Create;
using Frontliners.Inventory.Application.Products.Command.Update;
using Frontliners.Inventory.Application.Products.Dto;

namespace Frontliners.Inventory.IntegrationTest.Products;

public sealed class ProductsEndpointTest(CustomWebApplicationFactory<Program> factory)
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllProducts_Should_Return_Ok()
    {
        var response = await _client.GetAsync("/api/products");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateProduct_Should_Return_Created()
    {
        var command = new CreateProductCommand("Product 1", "Product 1 Description", 100, 10);

        var response = await _client.PostAsJsonAsync("/api/products", command);
        
        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        product.Should().NotBeNull();
        
        product.Name.Should().Be("Product 1");
       
    }
    
    [Fact]
    public async Task CreateProduct_Should_Return_BadRequest()
    {
        var command = new CreateProductCommand("Product 1", "Product 1 Description", -2, 10);

        var response = await _client.PostAsJsonAsync("/api/products", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateProduct_Should_Return_BadRequest_When_Quantity_Is_Negative()
    {
        var command = new CreateProductCommand("Product 1", "Product 1 Description", 100, -10);

        var response = await _client.PostAsJsonAsync("/api/products", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    [Fact]
    public async Task UpdateProduct_Should_Return_NoContent()
    {
        var command = new CreateProductCommand("Product 1", "Product 1 Description", 100, 10);

        var response = await _client.PostAsJsonAsync("/api/products", command);

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();

        var updateCommand = new UpdateProductCommand
        {
            ProductId = product.ProductId,
            Name = "Product 1 Updated",
            Description = "Product 1 Description Updated",
            Price = 200,
            Stock = 20
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/products/{product.ProductId}", updateCommand);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    
    
}
using System.Net;
using System.Net.Http.Json;
using InventoryManager.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore; // Added this
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace InventoryManager.Tests
{
    public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                // Ensure a fresh in-memory database for each test run
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<InventoryManager.API.Data.AppDbContext>(options =>
                        options.UseInMemoryDatabase("TestInventoryDb"));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            Assert.NotNull(products);
            Assert.True(products.Count >= 3); // based on seeded data
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Test Product",
                Price = 19.99M,
                Quantity = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", newProduct);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var created = await response.Content.ReadFromJsonAsync<Product>();

            Assert.NotNull(created);
            Assert.Equal(newProduct.Name, created!.Name);
            Assert.True(created.Id > 0);
        }

        [Fact]
        public async Task CreateProduct_InvalidData_ReturnsBadRequest()
        {
            // Arrange
            var invalidProduct = new Product
            {
                Name = "", // Invalid name
                Price = -1, // Invalid price
                Quantity = 0 // Invalid quantity
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/products", invalidProduct);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

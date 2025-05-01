using InventoryManager.API.Data;
using InventoryManager.API.Models;
using InventoryManager.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InventoryManager.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            // Configure DbContextOptions to use SQL Server
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=InventoryManagerTestDb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            _context = new AppDbContext(options);

            // Mock ILogger<ProductRepository>
            var mockLogger = new Mock<ILogger<ProductRepository>>();

            _repository = new ProductRepository(_context, mockLogger.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ReturnsAllProducts()
        {
            // Arrange
            _context.Products.Add(new Product { Name = "Test Product", Price = 10.99M, Quantity = 5 });
            _context.SaveChanges();

            // Act
            var products = await _repository.GetAllProductsAsync();

            // Assert
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task AddAsync_AddsProductToDatabase()
        {
            // Arrange
            var product = new Product { Name = "New Product", Price = 20.99M, Quantity = 10 };

            // Act
            var result = await _repository.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(1, await _context.Products.CountAsync());
        }
    }
}


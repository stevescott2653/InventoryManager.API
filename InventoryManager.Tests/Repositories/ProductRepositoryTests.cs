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
            // Configure DbContextOptions to use a unique in-memory database for each test
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a unique database name
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
        public async Task GetByIdAsync_ValidId_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.99M, Quantity = 5 };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999); // Non-existent ID

            // Assert
            Assert.Null(result);
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

        [Fact]
        public async Task UpdateAsync_ValidProduct_UpdatesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Original Product", Price = 10.99M, Quantity = 5 };
            _context.Products.Add(product);
            _context.SaveChanges();

            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Price = 15.99M, Quantity = 10 };

            // Act
            var result = await _repository.UpdateAsync(updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProduct.Name, result.Name);
            Assert.Equal(updatedProduct.Price, result.Price);
            Assert.Equal(updatedProduct.Quantity, result.Quantity);
        }

        [Fact]
        public async Task UpdateAsync_InvalidProduct_ReturnsNull()
        {
            // Arrange
            var updatedProduct = new Product { Id = 999, Name = "Non-existent Product", Price = 15.99M, Quantity = 10 };

            // Act
            var result = await _repository.UpdateAsync(updatedProduct);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_RemovesProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.99M, Quantity = 5 };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _repository.DeleteAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal(0, await _context.Products.CountAsync());
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteAsync(999); // Non-existent ID

            // Assert
            Assert.False(result);
        }
    }
}


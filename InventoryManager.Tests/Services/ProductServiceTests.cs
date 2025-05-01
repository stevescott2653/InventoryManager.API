using InventoryManager.API.Models;
using InventoryManager.API.Repositories.Interfaces;
using InventoryManager.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InventoryManager.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddAsync_ValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var product = new Product { Name = "Test Product", Price = 10.99M, Quantity = 5 };
            _mockRepository.Setup(repo => repo.AddAsync(product)).ReturnsAsync(product);

            // Act
            var result = await _productService.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            _mockRepository.Verify(repo => repo.AddAsync(product), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_InvalidId_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _productService.GetByIdAsync(0));
        }
    }
}


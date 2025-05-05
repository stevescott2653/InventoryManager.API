using InventoryManager.API.Models;
using InventoryManager.API.Repositories.Interfaces;
using InventoryManager.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManager.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching all products.");
            var products = await _repository.GetAllProductsAsync();
            _logger.LogInformation("Fetched {ProductCount} products.", products.Count());
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching product with ID {ProductId}.", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}.", id);
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
            }

            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
            }
            else
            {
                _logger.LogInformation("Fetched product with ID {ProductId}.", id);
            }

            return product;
        }

        public async Task<Product> AddAsync(Product product)
        {
            _logger.LogInformation("Adding a new product: {ProductName}.", product.Name);

            var createdProduct = await _repository.AddAsync(product);
            _logger.LogInformation("Product {ProductName} added successfully with ID {ProductId}.", createdProduct.Name, createdProduct.Id);

            return createdProduct;
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            _logger.LogInformation("Updating product with ID {ProductId}.", product.Id);

            if (product.Id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}.", product.Id);
                throw new ArgumentException("Product ID must be greater than zero.", nameof(product.Id));
            }

            var updatedProduct = await _repository.UpdateAsync(product);

            if (updatedProduct == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update.", product.Id);
            }
            else
            {
                _logger.LogInformation("Product with ID {ProductId} updated successfully.", product.Id);
            }

            return updatedProduct;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Deleting product with ID {ProductId}.", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid product ID: {ProductId}.", id);
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
            }

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion.", id);
            }
            else
            {
                _logger.LogInformation("Product with ID {ProductId} deleted successfully.", id);
            }

            return deleted;
        }
    }
}

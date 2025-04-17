using InventoryManager.API.Models;
using InventoryManager.API.Repositories.Interfaces;
using InventoryManager.API.Services.Interfaces;

namespace InventoryManager.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllProductsAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
            }

            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            ValidateProduct(product);

            return await _repository.AddAsync(product);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            if (product.Id <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", nameof(product.Id));
            }

            ValidateProduct(product);

            return await _repository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Product ID must be greater than zero.", nameof(id));
            }

            return await _repository.DeleteAsync(id);
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ArgumentException("Product name cannot be empty.", nameof(product.Name));
            }

            if (product.Price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.", nameof(product.Price));
            }

            if (product.Quantity < 0)
            {
                throw new ArgumentException("Product quantity cannot be negative.", nameof(product.Quantity));
            }
        }
    }
}
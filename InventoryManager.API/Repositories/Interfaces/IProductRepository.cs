using InventoryManager.API.Models;

namespace InventoryManager.API.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing product-related database operations.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A collection of products.</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// Retrieves a product by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the product.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The added product.</returns>
        Task<Product> AddAsync(Product product);

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product with updated details.</param>
        /// <returns>The updated product if successful; otherwise, null.</returns>
        Task<Product?> UpdateAsync(Product product);

        /// <summary>
        /// Deletes a product by its unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the product to delete.</param>
        /// <returns>True if the product was deleted; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}



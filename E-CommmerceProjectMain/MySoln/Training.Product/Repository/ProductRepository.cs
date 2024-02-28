using DataLayer.Data;
using DataLayer.Entities;

namespace Training.Product.Services
{
    public class ProductRepository : IProductRepository
    {
        public readonly ProductContext _context;
        public ProductRepository(ProductContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// Adds a new product to the database if it does not already exist.
        /// </summary>
        /// <param name="product">The product entity to be added.</param>
        /// <returns>A string indicating the outcome of the operation 
        /// ("Already Exist" if the product already exists, "Success" 
        /// if the product is successfully added).</returns>
        public string AddProduct(ProductEntity product)
        {
            var result = _context.Products.Where(u => u.ProductName == product.ProductName).FirstOrDefault();
            if (result != null)
            {
                return ("Already Exist");
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return ("Success");
        }

        /// <summary>
        /// Deletes a product from the database by its product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the product is successfully deleted; otherwise, false.</returns>
        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Finds and retrieves a product from the database by its product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to find.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// Returns the product entity if found; otherwise, null.</returns>
        public async Task<ProductEntity> FindProduct(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A list of all product entities.</returns>
        public List<ProductEntity> GetAllProducts()
        {
            List<ProductEntity> products = _context.Products.ToList();
            return products;
        }

        /// <summary>
        /// Retrieves distinct product categories from the database.
        /// </summary>
        /// <returns>A list of distinct product categories.</returns>
        public List<string> GetProductCategories()
        {
            var categories = _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToList();
            return categories;
        }

        /// <summary>
        /// Updates an existing product in the database with the data from the updated product entity.
        /// </summary>
        /// <param name="updatedProduct">The updated product entity.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the product is successfully updated; otherwise, false.</returns>
        public async Task<bool> UpdateProduct(ProductEntity updatedProduct)
        {
            var product = await _context.Products.FindAsync(updatedProduct.ProductId);

            if (product == null)
                return false;

            product.ProductName = updatedProduct.ProductName;
            product.Description = updatedProduct.Description;
            product.Quantity = updatedProduct.Quantity;
            product.Price = updatedProduct.Price;
            product.Discount = updatedProduct.Discount;
            product.Specification = updatedProduct.Specification;
            product.Data = updatedProduct.Data;
            product.Category = updatedProduct.Category;

            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Updates the quantity of a product in the database by subtracting the specified quantity of items.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantityOfItems">The quantity of items to subtract from the product's current quantity.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the product quantity is successfully updated; otherwise, false.</returns>
        public async Task<bool> UpdateProductQuantityAsync(int productId, int quantityOfItems)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                // Product not found
                return false;
            }

            // Update product quantity
            product.Quantity -= quantityOfItems;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

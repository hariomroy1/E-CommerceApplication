using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Training.Product.Services
{
    public class ProductRepository : IProductRepository
    {
        public readonly ProductContext _context;
        public ProductRepository(ProductContext context)
        {
            _context = context;
        }
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

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductEntity> FindProduct(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public List<ProductEntity> GetAllProducts()
        {
            List<ProductEntity> products = _context.Products.ToList();
            return products;
        }

        public List<string> GetProductCategories()
        {
            var categories = _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToList();
            return categories;
        }

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

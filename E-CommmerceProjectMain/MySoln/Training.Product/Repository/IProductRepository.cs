using DataLayer.Entities;

namespace Training.Product.Services
{
    public interface IProductRepository
    {
        string AddProduct(ProductEntity product);
        List<ProductEntity> GetAllProducts();
        Task<ProductEntity> FindProduct(int productId);
        Task<bool> DeleteProduct(int productId);
        Task<bool> UpdateProduct(ProductEntity updatedProduct);
        List<string> GetProductCategories();
        Task<bool> UpdateProductQuantityAsync(int productId, int quantityOfItems);

    }
}

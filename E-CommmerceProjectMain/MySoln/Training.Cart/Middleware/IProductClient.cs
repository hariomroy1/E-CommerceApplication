using DataLayer.Entities;

namespace Training.Cart.Middleware
{
    public interface IProductClient
    {
        Task<ProductEntity> GetProduct(int productId);
    }
}
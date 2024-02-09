using DataLayer.Entities;
using Training.Cart.Model;

namespace Training.Cart.Services
{
    public interface ICartRepository
    {
        Task<List<object>> GetCartItems(int registerId);
        Task<bool> AddToCart(CartDTO cartObj);
        Task<bool> RemoveItemFromCart(int productId, int userId);
        Task<CartEntity> FindCartItem(int cartId);

        Task<bool> ClearCart(int userId);

    }
}

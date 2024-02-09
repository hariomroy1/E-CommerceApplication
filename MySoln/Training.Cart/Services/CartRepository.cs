using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Cart.Model;

namespace Training.Cart.Services
{
    public class CartRepository : ICartRepository
    {
        public readonly CartContext _context;
        string _apiUrl = "https://localhost:7186/api/Product/FindProduct/";
        string apiURL = "https://localhost:7048/api/Register/CurrentUserById/";

        public CartRepository(CartContext context)
        {
            _context = context;
        }

        internal bool ItemAlreadyInCart(int productId, int registerId)
        {
            var cartItem = _context.Carts.FirstOrDefault(x => x.ProductId == productId && x.RegisterId == registerId);
            return cartItem != null;
        }

        private async Task<ProductEntity> GetProduct(int productId)
        {
            string productEndpointUrl = _apiUrl + productId;
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(productEndpointUrl);
            var prod1 = await httpClient.GetFromJsonAsync<ProductEntity>(productEndpointUrl);
            return prod1;
        }

        private async Task<RegisterEntity> GetUser(int userId)
        {
            string productEndpointUrl = apiURL + userId;
            var httpClient = new HttpClient();

            var prod1 = await httpClient.GetFromJsonAsync<RegisterEntity>(productEndpointUrl);

            return prod1;
        }


        public async Task<bool> AddToCart([FromBody]CartDTO cartObj)
        {
            if (ItemAlreadyInCart(cartObj.ProductId, cartObj.RegisterId) == false)
            {
                try
                {
                    CartEntity cart = new CartEntity
                    {
                        ProductId = cartObj.ProductId,
                        RegisterId = cartObj.RegisterId,
                        Quantity = cartObj.Quantity
                    };
                    await _context.Carts.AddAsync(cart);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<List<object>> GetCartItems(int registerId)
        {
            var cartItems = await _context.Carts
        .Where(x => x.RegisterId == registerId)
        .ToListAsync();

            var result = new List<object>();

            foreach (var cartItem in cartItems)
            {
                // Assuming you have an endpoint in the Product microservice to get product details by Id
                var productDetails = await GetProduct(cartItem.ProductId);

                // Create a new object with combined details
                var cartItemDetails = new
                {
                    Product = productDetails,
                    Quantity = cartItem.Quantity
                };
                result.Add(cartItemDetails);
            }
            return result;
        }

        public async Task<bool> RemoveItemFromCart(int productId,int userId)
        {
            var itemsToBeDeleted = _context.Carts.Where(x => x.ProductId == productId && x.RegisterId == userId);
            if (!itemsToBeDeleted.Any())
            {
                return false;
            }
            foreach (var item in itemsToBeDeleted)
                _context.Carts.Remove(item);
            _context.SaveChanges();

            return true;
        }
        public async Task<CartEntity> FindCartItem(int cartId)
        {
            return await _context.Carts.FindAsync(cartId);
        }

        public async Task<bool> ClearCart(int userId)
        {
            var itemsToBeDeleted = _context.Carts.Where(x => x.RegisterId == userId);

            if (!itemsToBeDeleted.Any())
            {
                // No items found for the given userId
                return false;
            }

            foreach (var item in itemsToBeDeleted)
            {
                _context.Carts.Remove(item);
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}

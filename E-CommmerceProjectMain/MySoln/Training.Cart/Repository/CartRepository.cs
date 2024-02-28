using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.Cart.Middleware;
using Training.Cart.Model;

namespace Training.Cart.Repository
{
    public class CartRepository : ICartRepository
    {
        public readonly CartContext _context;
        public readonly ProductClient _productClient;
      
        string apiURL = "http://localhost:5293/api/Register/CurrentUserById/";

        public CartRepository(CartContext context,ProductClient productClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _productClient = productClient ?? throw new ArgumentNullException("products.");

        }


        /// <summary>
        /// Checks if a specific product is already present in the cart of a user identified by their register ID.
        /// </summary>
        /// <param name="productId">The ID of the product to check.</param>
        /// <param name="registerId">The ID of the user (register) whose cart is to be checked.</param>
        /// <returns>True if the product is already in the cart; otherwise, false.</returns>
        internal bool ItemAlreadyInCart(int productId, int registerId)
        {
            var cartItem = _context.Carts.FirstOrDefault(x => x.ProductId == productId && x.RegisterId == registerId);
            return cartItem != null;

        }


        /// <summary>
        /// Retrieves user information from an external API based on the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns the user entity if found; otherwise, null.</returns>
        private async Task<RegisterEntity> GetUser(int userId)
        {
            string productEndpointUrl = apiURL + userId;
            var httpClient = new HttpClient();

            var prod1 = await httpClient.GetFromJsonAsync<RegisterEntity>(productEndpointUrl);

            return prod1;
        }

        /// <summary>
        /// Adds a product to the cart of a user identified by their register ID.
        /// </summary>
        /// <param name="cartObj">The cart DTO containing information about the product to be added.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the product is successfully added to the cart; otherwise, false.</returns>
        public async Task<bool> AddToCart([FromBody] CartDTO cartObj)
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

        /// <summary>
        /// Retrieves the items in the cart of a user identified by their register ID.
        /// </summary>
        /// <param name="registerId">The ID of the user (register) whose cart items are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// Returns a list of objects containing product details and quantities in the cart.</returns>
        public async Task<List<object>> GetCartItems(int registerId)
        {
            var cartItems = await _context.Carts
        .Where(x => x.RegisterId == registerId)
        .ToListAsync();

            var result = new List<object>();

            foreach (var cartItem in cartItems)
            {
                // Assuming you have an endpoint in the Product microservice to get product details by Id
                var productDetails = await _productClient.GetProduct(cartItem.ProductId);

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

        /// <summary>
        /// Removes a specific product item from the cart of a user identified by their register ID.
        /// </summary>
        /// <param name="productId">The ID of the product item to remove from the cart.</param>
        /// <param name="userId">The ID of the user (register) whose cart is to be modified.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the product item is successfully removed from the cart; otherwise, false.</returns>
        public async Task<bool> RemoveItemFromCart(int productId, int userId)
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
        /// <summary>
        /// Finds and retrieves a cart item from the database by its cart ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart item to find.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns the cart entity if found; otherwise, null.</returns>
        public async Task<CartEntity> FindCartItem(int cartId)
        {
            return await _context.Carts.FindAsync(cartId);
        }

        /// <summary>
        /// Clears all items from the cart of a user identified by their register ID.
        /// </summary>
        /// <param name="userId">The ID of the user (register) whose cart is to be cleared.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the cart is successfully cleared; otherwise, false.</returns>

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

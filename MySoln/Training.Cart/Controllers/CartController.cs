using DataLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Training.Cart.Model;
using Training.Cart.Services;

namespace Training.Cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CartController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly CartContext _context;

        public CartController(IConfiguration configuration, ICartRepository cartRepository)
        {
            _configuration = configuration;
            _cartRepository = cartRepository;
        }

        /// <summary>
        /// Adds a product to the user's cart.
        /// </summary>
        /// <param name="cartObj">The CartDTO containing information about the product to be added.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If the product is successfully added to the cart, returns Ok with a success message.
        /// If the product is already present in the cart, returns Ok with a message indicating its presence.
        /// </returns>
        /// <exception>Thrown when the provided CartDTO is null,
        /// indicating the absence of the product.</exception>

        [HttpPost("AddToCart")]
        //[Authorize(Roles ="user")]
        public async Task<IActionResult> AddToCart([FromBody] CartDTO cartObj)
        {
            if (cartObj == null)
               return BadRequest();

            var res = await _cartRepository.AddToCart(cartObj);

            if (res == true)
            {
                return Ok(new { Message = "Product Added to the cart!" });
            }

            return Ok(new { Message = "This product is already in your cart!" });
        }

        /// <summary>
        /// Finds and retrieves cart items based on the provided cartId.
        /// </summary>
        /// <param name="cartId">The identifier of the cart for which to retrieve items.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If cart items are found, returns Ok with the cart items.
        /// If no cart items are found, throws an InvalidOperationException with a message indicating their absence.
        /// </returns>
        /// <exception>Thrown when no products are found in the cart 
        /// with the specified cartId.</exception>

        [HttpGet("FindCartItems/{cartId}")]
       // [Authorize(Roles = "user")]
        public async Task<IActionResult> FindCartItems(int cartId)
        {
            var cartItem = await _cartRepository.FindCartItem(cartId);
            if (cartItem == null)
            {
                throw new InvalidOperationException($"Any Product is not present in Cart with cartId: {cartId}");
            }

            return Ok(cartItem);
        }


        /// <summary>
        /// Retrieves cart items based on the provided registerId.
        /// </summary>
        /// <param name="registerId">The identifier of the user register for which to retrieve cart items.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If cart items are found, returns Ok with the cart items.
        /// If no cart items are found, throws an InvalidOperationException with a message indicating their absence.
        /// </returns>
        /// <exception>Thrown when no products are found 
        /// in the cart with the specified registerId.</exception>

        [HttpGet("GetCartItems/{registerId}")]
       // [Authorize(Roles = "user")]
        public async Task<IActionResult> GetCartItems(int registerId)

        {
            var result = await _cartRepository.GetCartItems(registerId);
            if (result.Count > 0)
            {
                return Ok(result);
            }
            throw new InvalidOperationException($"Any Product is not present in Cart with registerId: {registerId}");

        }

        /// <summary>
        /// Removes a product item from the user's cart based on the provided userId and productId.
        /// </summary>
        /// <param name="productId">The identifier of the product to be removed from the cart.</param>
        /// <param name="userId">The identifier of the user's cart from which to remove the product item.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If the item is successfully removed, returns Ok with a success message.
        /// If the provided userId is null or no item is found to remove, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when the provided userId is null or no product item is found in the cart with the specified userId.
        /// </exception>

        [HttpDelete("RemoveItemFromCart/{productId}/{userId}")]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> RemoveItemFromCart(int productId, int userId)
        {

            if (userId == null)
            {
                throw new InvalidOperationException($"Any Product is not present in Cart with userId: {userId}");
            }

            var result = await _cartRepository.RemoveItemFromCart(productId,userId);
            if (result == true)
            {
                return Ok(new { Message = "Items removed" });
            }
            else
            {
                throw new InvalidOperationException($"Any Product is not present in Cart with userId: {userId}");
            }
        }


        /// <summary>
        /// Clears all product items from the user's cart based on the provided userId.
        /// </summary>
        /// <param name="userId">The identifier of the user's cart to be cleared.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If the cart is successfully cleared, returns Ok with a success message.
        /// If no product items are found in the cart with the specified userId, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when no product items are found in the cart with the specified userId.
        /// </exception>

        [HttpDelete("clear/{userId}")]
        //[Authorize(Roles = "user")]
        public async Task<IActionResult> ClearCart(int userId)
        {

            var result = await _cartRepository.ClearCart(userId);
            if (result == false)
            {
                throw new InvalidOperationException($"Any Product is not present in Cart with registerId: {userId}");
            }
            return Ok("Cart cleared successfully.");

        }
    }
}

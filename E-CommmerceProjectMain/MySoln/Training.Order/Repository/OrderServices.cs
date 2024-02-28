using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Training.Order.Model;

namespace Training.Order.Services
{
    public class OrderServices : IOrderServices
    {
        public readonly OrderContext _context;
        string _apiUrl = "http://localhost:7186/api/Product/FindProduct/";
        string ForClearCartUrl = "http://localhost:7147/api/Cart/clear/";
        string clearParticularItemFromCart = "http://localhost:7147/api/Cart/RemoveItemFromCart";
        string apiURL = "http://localhost:5293/api/Register/CurrentUserById/";
        string UpdateProductQuantityUrl = "http://localhost:7186/api/Product/UpdateProductQuantity";
        string cartItemUrl = "http://localhost:7147/api/Cart/FindCartItems/";
        public OrderServices(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Retrieves product information from an external API based on the specified product ID.
        /// </summary>
        /// <param name="productId">The ID of the product to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns the product entity if found; otherwise, null.</returns>
        private async Task<ProductEntity> GetProduct(int productId)
        {
            string productEndpointUrl = _apiUrl + productId;
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(productEndpointUrl);

            var prod1 = await httpClient.GetFromJsonAsync<ProductEntity>(productEndpointUrl);
            Console.WriteLine(prod1);
            return prod1;
        }

        /// <summary>
        /// Retrieves cart item information from an external API based on the specified cart ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart item to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns the cart item entity if found; otherwise, null.</returns>
        private async Task<CartEntity> GetCartItem(int cartId)
        {
            string productEndpointUrl = cartItemUrl + cartId;
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(productEndpointUrl);

            var prod1 = await httpClient.GetFromJsonAsync<CartEntity>(productEndpointUrl);
            Console.WriteLine(prod1);
            return prod1;
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
        /// Retrieves a list of orders placed by a user identified by the specified register ID.
        /// </summary>
        /// <param name="registerId">The ID of the user (register) whose orders are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns a list of order DTOs representing the orders placed by the user.</returns>
        public async Task<List<OrderDTO>> GetOrders(int registerId)
        {
            var orders = await _context.Orders.Where(x => x.RegisterId == registerId).ToListAsync();
            var orderList = new List<OrderDTO>();

            foreach (var order in orders)
            {
                var orderDto = new OrderDTO();
                orderDto.RegisterId = order.RegisterId;
                orderDto.OrderDate = order.OrderDate;
                orderDto.TotalPrice = order.TotalPrice;
                orderDto.QuantityOfItems = order.QuantityOfItems;

                orderList.Add(orderDto);
            }

            return orderList;
        }

        /// <summary>
        /// Clears the cart associated with the user identified by the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is to be cleared.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task ClearCart(int userId)
        {
            string productEndpointUrl = ForClearCartUrl + userId;
            var httpClient = new HttpClient();
            var prod1 = await httpClient.DeleteAsync(productEndpointUrl);
        }

        /// <summary>
        /// Clears a particular item from the cart associated with the user identified by the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item is to be cleared.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task ClearCartProduct(int userId)
        {
            string productEndpointUrl = $"{clearParticularItemFromCart}/{userId}";
            var httpClient = new HttpClient();

            var prod1 = await httpClient.DeleteAsync(productEndpointUrl);
        }

        /// <summary>
        /// Updates the quantity of a product in the product inventory asynchronously.
        /// </summary>
        /// <param name="productId">The ID of the product to update.</param>
        /// <param name="quantityofItems">The quantity of items to update for the product.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        internal async Task UpdateProductQuantityAsync(int productId, int quantityofItems)
        {
            string productEndpointUrl = $"{UpdateProductQuantityUrl}/{productId}/{quantityofItems}";
            var httpClient = new HttpClient();

            // Assuming you have an object to send as JSON in the request body
            var data = new
            {
                QuantityOfItems = quantityofItems
            };

            // Use the data parameter to include the object in the request body
            var response = await httpClient.PutAsJsonAsync(productEndpointUrl, data);

            // Optionally, you can check the response, handle errors, etc.
        }

        /// <summary>
        /// Places an order for a product and updates the product inventory and user's cart accordingly.
        /// </summary>
        /// <param name="orderObj">The order DTO containing order details.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// Returns true if the order is successfully placed; otherwise, false.</returns>
        public async Task<bool> PlaceOrder(OrderDTO orderObj)
        {
            try
            {
                // Get user and product information
                //var user = await GetUser(orderObj.RegisterId);


                await _context.SaveChangesAsync();

                // Create and save the order
                OrderEntity order = new OrderEntity
                {
                    RegisterId = orderObj.RegisterId,
                    TotalPrice = (int)orderObj.TotalPrice,
                    ProductId = orderObj.productId,
                    QuantityOfItems = orderObj.QuantityOfItems,
                    OrderDate = DateTime.Now,
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // Update product quantity
                UpdateProductQuantityAsync(orderObj.productId, orderObj.QuantityOfItems);

                //clear cart
                ClearCart(orderObj.RegisterId);

                return true;



            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return false;
            }
        }
    }
}

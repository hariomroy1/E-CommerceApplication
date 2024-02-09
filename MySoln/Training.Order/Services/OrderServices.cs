using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Training.Order.Model;

namespace Training.Order.Services
{
    public class OrderServices : IOrderServices
    {
        public readonly OrderContext _context;
        string _apiUrl = "https://localhost:7186/api/Product/FindProduct/";
        string ForClearCartUrl = "https://localhost:7147/api/Cart/clear/";
        string clearParticularItemFromCart = "https://localhost:7147/api/Cart/RemoveItemFromCart";
        string apiURL = "https://localhost:7226/api/Register/CurrentUserById/";
        string UpdateProductQuantityUrl = "https://localhost:7186/api/Product/UpdateProductQuantity";
        string cartItemUrl = "https://localhost:7147/api/Cart/FindCartItems/";
        public OrderServices(OrderContext context)
        {
            _context = context;
        }

        private async Task<ProductEntity> GetProduct(int productId)
        {
            string productEndpointUrl = _apiUrl + productId;
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(productEndpointUrl);

            var prod1 = await httpClient.GetFromJsonAsync<ProductEntity>(productEndpointUrl);
            Console.WriteLine(prod1);
            return prod1;
        }
        private async Task<CartEntity> GetCartItem(int cartId)
        {
            string productEndpointUrl = cartItemUrl + cartId;
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(productEndpointUrl);

            var prod1 = await httpClient.GetFromJsonAsync<CartEntity>(productEndpointUrl);
            Console.WriteLine(prod1);
            return prod1;
        }
        private async Task<RegisterEntity> GetUser(int userId)
        {
            string productEndpointUrl = apiURL + userId;
            var httpClient = new HttpClient();

            //var response = await httpClient.GetAsync(productEndpointUrl);

            var prod1 = await httpClient.GetFromJsonAsync<RegisterEntity>(productEndpointUrl);

            return prod1;
        }
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

        internal async Task ClearCart(int userId)
        {
            string productEndpointUrl = ForClearCartUrl + userId;
            var httpClient = new HttpClient();
            var prod1 = await httpClient.DeleteAsync(productEndpointUrl);
        }

        internal async Task ClearCartProduct(int userId)
        {
            string productEndpointUrl = $"{clearParticularItemFromCart}/{userId}";
            var httpClient = new HttpClient();

            var prod1 = await httpClient.DeleteAsync(productEndpointUrl);
        }


        internal async Task UpdateProductQuantityAsync(int productId,int quantityofItems)
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
                    UpdateProductQuantityAsync(orderObj.productId,orderObj.QuantityOfItems);

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

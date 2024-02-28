using DataLayer.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Training.Order.Model;
using Training.Order.Services;

namespace Training.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IOrderServices _orderServices;
        private readonly OrderContext _context;

        public OrderController(IConfiguration configuration, IOrderServices orderServices)
        {
            _configuration = configuration;
            _orderServices = orderServices;
        }

        /// <summary>
        /// Retrieves the orders associated with the specified registerId.
        /// </summary>
        /// <param name="registerId">The identifier of the registered user whose orders are to be retrieved.</param>
        /// <returns>
        /// An IActionResult representing the result of the operation.
        /// If orders are found for the specified registerId, returns Ok with the list of orders.
        /// If no orders are found, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when no orders are found for the specified registerId.
        /// </exception>

        [HttpGet("GetOrders")]

        public async Task<IActionResult> GetOrders(int registerId)
        {
            var result = await _orderServices.GetOrders(registerId);
            if (result.Count == 0)
            {
                throw new InvalidOperationException($"We have No orders with registerId: {registerId}");
            }
            return Ok(result);
        }

        /// <summary>
        /// Places an order based on the provided OrderDTO.
        /// </summary>
        /// <param name="orderObj">The OrderDTO containing order details.</param>
        /// <returns>
        /// An IActionResult representing the result of the order placement operation.
        /// If the order is placed successfully, returns Ok with a success message.
        /// If the order placement fails or the provided OrderDTO is null, throws an InvalidOperationException.
        /// </returns>
        /// <exception>
        /// Thrown when the order placement fails or the provided OrderDTO is null.
        /// </exception>

        [HttpPost("PlaceOrder")]



        public async Task<IActionResult> PlaceOrder([FromBody] OrderDTO orderObj)
        {
            if (orderObj == null)
            {
                throw new InvalidOperationException($"Ordered is not placed yet");
            }
            var res = await _orderServices.PlaceOrder(orderObj);

            if (res == true)
            {
                return Ok(new { Message = "Order placed successfully!" });
            }
            throw new InvalidOperationException($"Ordered is not placed yet");
        }
    }
}

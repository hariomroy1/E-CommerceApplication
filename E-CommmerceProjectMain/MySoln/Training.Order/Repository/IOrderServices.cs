using Training.Order.Model;

namespace Training.Order.Services
{
    public interface IOrderServices
    {
        Task<List<OrderDTO>> GetOrders(int registerId);
        Task<bool> PlaceOrder(OrderDTO orderObj);
    }
}

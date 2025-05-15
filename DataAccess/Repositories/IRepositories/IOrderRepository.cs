
namespace DataAccess.Repositories.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetBySessionIdAsync(string sessionId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }

}

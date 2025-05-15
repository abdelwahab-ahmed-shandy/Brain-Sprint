
namespace DataAccess.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}

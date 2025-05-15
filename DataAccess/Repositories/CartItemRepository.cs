
namespace DataAccess.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}

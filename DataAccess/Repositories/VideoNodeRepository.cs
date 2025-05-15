
namespace DataAccess.Repositories
{
    public class VideoNodeRepository : Repository<VideoNode>, IVideoNodeRepository
    {
        public VideoNodeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}

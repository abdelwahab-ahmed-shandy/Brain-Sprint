
namespace DataAccess.Repositories
{
    public class CertificateRepository : Repository<Certificate>, ICertificateRepository
    {
        public CertificateRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}

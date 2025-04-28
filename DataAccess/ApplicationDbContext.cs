namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        #region Entities definition :

        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        #endregion




    }
}

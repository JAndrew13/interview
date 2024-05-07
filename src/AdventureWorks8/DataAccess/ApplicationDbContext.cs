using AdventureWorks.Models;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.DataAccess
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<UserModel> Users { get; set; }
    }


}
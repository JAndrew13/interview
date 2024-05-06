using AdventureWorks.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AdventureWorks.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(new ApplicationDbContextInitializer());
        }


        public DbSet<UserModel> Users { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }


}
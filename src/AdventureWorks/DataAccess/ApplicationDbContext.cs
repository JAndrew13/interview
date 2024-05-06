using AdventureWorks.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AdventureWorks.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            

            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
        }

        protected void CreateTables()
        {
            const string sqlTextCreateTables = @"
        CREATE TABLE IF NOT EXISTS AspNetUsers
        (
            Id TEXT PRIMARY KEY NOT NULL,
            Email TEXT,
            EmailConfirmed BIT,
            PasswordHash TEXT,
            SecurityStamp TEXT,
            PhoneNumber TEXT,
            PhoneNumberConfirmed BIT,
            TwoFactorEnabled BIT,
            LockoutEndDateUtc DATETIME,
            LockoutEnabled BIT,
            PostalCode TEXT,
            AccessFailedCount INT,
            UserName TEXT
        );
        CREATE TABLE IF NOT EXISTS AspNetUserClaims
        (
            Id TEXT PRIMARY KEY NOT NULL,
            UserId TEXT,
            ClaimType BIT,
            ClaimValue TEXT
        );
        CREATE TABLE IF NOT EXISTS AspNetUserLogins
        (
            LoginProvider TEXT,
            ProviderKey TEXT,
            UserId TEXT
        );
        CREATE TABLE IF NOT EXISTS AspNetRoles
        (
            Id TEXT,
            Name TEXT
        );
        CREATE TABLE IF NOT EXISTS AspNetUserRoles
        (
            UserId TEXT,
            RoleId TEXT
        );
        ";

            var connectionString = this.Database.Connection.ConnectionString;
            using (var dbConnection = new System.Data.SQLite.SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                using (var dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = sqlTextCreateTables;
                    dbCommand.ExecuteNonQuery();
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.CreateTables();
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
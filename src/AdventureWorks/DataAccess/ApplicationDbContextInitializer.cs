using System;
using System.Data.Entity;
using System.IO;
using System.Net.Http;
using System.Web;

namespace AdventureWorks.DataAccess
{
    public class ApplicationDbContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        public override void InitializeDatabase(ApplicationDbContext context)
        {
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/"), "App_Data");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "account.sqlite");
            if (!File.Exists(path))
            {
                using (var fs = File.Create(path))
                {
                    fs.Close();
                }
                Seed(context);
            }
        }

        protected override void Seed(ApplicationDbContext context)
        {
            const string sqlTextCreateTables = @"
        CREATE TABLE IF NOT EXISTS User
        (
            Id TEXT PRIMARY KEY NOT NULL,
            Email TEXT
        );
        ";

            var connectionString = context.Database.Connection.ConnectionString;
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

    }


}
using AdventureWorks.DataAccess;
using System;
using System.Linq;

namespace AdventureWorks.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddUsers()
        {
            foreach(var i in Enumerable.Range(0, 5))
            {
                _context.Users.Add(new Models.ApplicationUser()
                {
                    UserName = Guid.NewGuid().ToString(),
                    Email = $"{Guid.NewGuid()}@test.com",
                });
            }
            _context.SaveChanges();
        }

        public int GetUserCount()
        {
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Session["USER_COUNT"] == null ||
                    !int.TryParse(System.Web.HttpContext.Current.Session["USER_COUNT"].ToString(), out var count))
                {
                    count = _context.Users.Count();
                    System.Web.HttpContext.Current.Session["USER_COUNT"] = count;
                }
                return count;
            }
            return _context.Users.Count();
        }

    }
}
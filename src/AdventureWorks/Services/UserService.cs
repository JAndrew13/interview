using AdventureWorks.DataAccess;
using AdventureWorks.Models;
using System;
using System.Collections.Generic;
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

        public IEnumerable<UserModel> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUsers(int count)
        {
            foreach(var i in Enumerable.Range(0, count))
            {
                _context.Users.Add(new Models.UserModel()
                {
                    Id = Guid.NewGuid().ToString(),
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
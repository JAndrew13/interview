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
            return _context.Users.Count();
        }

    }
}
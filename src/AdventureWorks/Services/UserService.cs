using AdventureWorks.DataAccess;
using AdventureWorks.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AdventureWorks.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public UserService(ApplicationDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void AddUsers(int count)
        {
            foreach (var i in Enumerable.Range(0, count))
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
            if (_memoryCache.TryGetValue("USER_COUNT", out int value))
            {
                return value;
            }
            else
            {
                var userCount = _context.Users.Count();
                _memoryCache.Set("USER_COUNT", userCount);
                return userCount;
            }
        }

    }
}
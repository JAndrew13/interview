using AdventureWorks.Models;
using AdventureWorks.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorks.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserService _userService;

        public IndexModel(ILogger<IndexModel> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public int Count { get; set; }
        public IEnumerable<UserModel> Users { get; set; } = Enumerable.Empty<UserModel>();

        public void OnGet()
        {
            Count = _userService.GetUserCount();
            Users = _userService.GetUsers();
        }

        public void OnPost(int count)
        {
            _userService.AddUsers(count);
            Count = _userService.GetUserCount();
            Users = _userService.GetUsers();
        }
    }
}

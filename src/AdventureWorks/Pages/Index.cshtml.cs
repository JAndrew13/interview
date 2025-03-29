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
        public List<UserModel> Users { get; set; } = new List<UserModel>();
        
        public void OnGet()
        {
            Users = _userService.GetAllUsers();  // Fetch a list of all users
            Count = _userService.GetUserCount();
        }

    }
}

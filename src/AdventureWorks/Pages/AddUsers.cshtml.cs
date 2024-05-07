using AdventureWorks.Models;
using AdventureWorks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdventureWorks.Pages
{
    public class AddUsersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserService _userService;

        public AddUsersModel(ILogger<IndexModel> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public int Count { get; set; }
        public IEnumerable<UserModel> Users { get; set; } = Enumerable.Empty<UserModel>();

        public void OnGet()
        {
            Users = _userService.GetUsers();
            Count = Users.Count();

        }

        public void OnPost(int count)
        {
            _userService.AddUsers(count);
            Users = _userService.GetUsers();
            Count = Users.Count();
        }
    }
}

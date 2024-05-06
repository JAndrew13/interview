using AdventureWorks.DataAccess;
using AdventureWorks.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdventureWorks.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController()
        {
            _applicationDbContext = ApplicationDbContext.Create();
        }


        public HomeController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult CreateUsers()
        {
            var userService = new UserService(_applicationDbContext);
            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            if ((System.Web.HttpContext.Current?.User.Identity.IsAuthenticated).GetValueOrDefault())
            {
                ViewBag.Message += $"  {System.Web.HttpContext.Current.User.Identity.Name} is logged in";
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUsersPost()
        {
            var userService = new UserService(_applicationDbContext);
            
            await Task.Run(() =>
            {
                userService.AddUsers();
            }).ConfigureAwait(false);

            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            if ((System.Web.HttpContext.Current?.User.Identity.IsAuthenticated).GetValueOrDefault())
            {
                ViewBag.Message += $"  {System.Web.HttpContext.Current.User.Identity.Name} is logged in";
            }

            return View("CreateUsers");
        }

    }
}
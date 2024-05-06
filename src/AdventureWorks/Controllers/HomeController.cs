using AdventureWorks.DataAccess;
using AdventureWorks.Models;
using AdventureWorks.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdventureWorks.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }


        [HttpGet]
        public ActionResult Index()
        {
            var userService = new UserService(ApplicationDbContext.Create());
           
            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            if ((System.Web.HttpContext.Current?.User.Identity.IsAuthenticated).GetValueOrDefault())
            {
                ViewBag.Message += $"  {System.Web.HttpContext.Current.User.Identity.Name} is logged in";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            var userService = new UserService(ApplicationDbContext.Create());

            userService.AddUsers(model.Count);

            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            if ((System.Web.HttpContext.Current?.User.Identity.IsAuthenticated).GetValueOrDefault())
            {
                ViewBag.Message += $"  {System.Web.HttpContext.Current.User.Identity.Name} is logged in";
            }

            return View();
        }

    }
}
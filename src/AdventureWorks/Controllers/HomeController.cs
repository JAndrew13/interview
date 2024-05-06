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

            return View();
        }

        [HttpPost]
        public ActionResult Index(IndexModel model)
        {
            var userService = new UserService(ApplicationDbContext.Create());

            userService.AddUsers(model.Count);

            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            return View();
        }

    }
}
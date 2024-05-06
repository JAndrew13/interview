using AdventureWorks.DataAccess;
using AdventureWorks.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdventureWorks.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {
            var userService = new UserService(ApplicationDbContext.Create());
            await Task.Run(() =>
            {

                userService.AddUsers();
            }).ConfigureAwait(false);

            var count = userService.GetUserCount();

            ViewBag.Message = $"{count} users.";

            ViewBag.Message += $"  {System.Web.HttpContext.Current?.User.Identity.Name} is logged in";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
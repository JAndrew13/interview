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
           
            var response = new IndexResponseModel();
            response.Count = userService.GetUserCount();

            return View(response);
        }

        [HttpPost]
        public ActionResult Index(IndexRequestModel model)
        {
            var userService = new UserService(ApplicationDbContext.Create());

            userService.AddUsers(model.Count);

            var response = new IndexResponseModel();

            response.Count = userService.GetUserCount();
            response.Users = userService.GetUsers();

            return View(response);
        }

    }
}
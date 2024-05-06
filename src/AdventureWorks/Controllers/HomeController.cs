using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
            await Task.Run(() => 
            {
                //do something
            }).ConfigureAwait(false);
            if ((System.Web.HttpContext.Current?.User.Identity.IsAuthenticated).GetValueOrDefault())
            {
                ViewBag.Message = $"{System.Web.HttpContext.Current.User.Identity.Name} is logged in";
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
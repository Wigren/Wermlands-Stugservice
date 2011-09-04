using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class EnController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "MotherEffin HTML5 Boilerplate MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        
        public ActionResult Services()
        {
            return View();
        }

    }
}

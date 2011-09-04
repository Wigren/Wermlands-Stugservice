using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class SvController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "MotherEffin HTML5 Boilerplate MVC!";

            return View();
        }

        public ActionResult Om()
        {
            return View();
        }
        
        public ActionResult Tjanster()
        {
            return View();
        }

    }
}

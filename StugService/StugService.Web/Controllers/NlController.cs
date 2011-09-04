using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class NlController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "MotherEffin HTML5 Boilerplate MVC!";

            return View();
        }

        public ActionResult Over()
        {
            return View();
        }
        
        public ActionResult Service()
        {
            return View();
        }

    }
}

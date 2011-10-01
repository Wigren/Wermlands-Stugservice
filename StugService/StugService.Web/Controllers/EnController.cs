using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class EnController : Base.ControllerBase
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

        public override ActionResult Thanks()
        {
            return View();
        }
    }
}

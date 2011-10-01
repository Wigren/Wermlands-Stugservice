using System.Web.Mvc;
using StugService.Web.Models;

namespace StugService.Web.Controllers
{
    public class SvController : Base.ControllerBase
    {
        public ActionResult Index()
        {
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

        public override ActionResult Tack()
        {
            return View();
        }
    }
}

using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class NlController : Base.ControllerBase
    {
        public ActionResult Index()
        {
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
        public override ActionResult Bedankt()
        {
            return View();
        }
    }
}

using System.Web.Mvc;

namespace StugService.Web.Controllers
{
    public class DeController : Base.ControllerBase
    {
        //
        // GET: /De/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Uber()
        {
            return View();
        }

        public ActionResult Angebote()
        {
            return View();
        }

        public override ActionResult Danke()
        {
            return View();
        }
    }
}

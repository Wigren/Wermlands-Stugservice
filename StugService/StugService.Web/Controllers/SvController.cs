using System;
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
            ViewData["Changeing"] = DateTime.Now.ToString();
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

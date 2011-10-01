using System.Web.Mvc;
using StugService.Web.Models;

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

        public ActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel contactVM)
        {
            if (!ModelState.IsValid)
            {
                return View(contactVM);
            }

            var contact = new Contact
            {
                From = contactVM.From,
                Message = contactVM.Message
            };

            new Email().Send(contact);

            return RedirectToAction("ContactConfirm");
        }

        public ActionResult ContactConfirm()
        {
            return View();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StugService.Web.Common;
using StugService.Web.Extensions;
using StugService.Web.Models;

namespace StugService.Web.Controllers.Base
{
    public class ControllerBase : Controller
    {

        [HttpPost]
        public ActionResult Contact(ContactViewModel contactVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(contactVm);
                }

                var contact = new Contact
                {
                    From = contactVm.From,
                    Message = contactVm.Message
                };

                new Email().Send(contact);

                switch (Request.CurrentLang())
                {
                    case Lang.De:
                        return RedirectToAction("Danke");
                    case Lang.Nl:
                        return RedirectToAction("Bedankt");
                    case Lang.En:
                        return RedirectToAction("Thanks");
                    default:
                        return RedirectToAction("Tack");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public virtual ActionResult Danke()
        {
            return null;
        }
        
        public virtual ActionResult Bedankt()
        {
            return null;
        }
        public virtual   ActionResult Thanks()
        {
            return null;
        }
        public virtual ActionResult Tack()
        {
            return null;
        }

    }
}
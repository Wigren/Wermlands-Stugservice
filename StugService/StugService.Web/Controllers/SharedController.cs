using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MailgunClient;
using StugService.Web.Models;

namespace StugService.Web.Controllers
{
    public class SharedController : Controller
    {
        private const string MailTo = "christian@wigren.se";

        [AcceptVerbs("POST")]
        public ActionResult SendEmail(ContactFormInputModel model, string returnUrl)
        {
            Mailgun.Init("key-afy6amxoo2fnj$u@mc");

            try
            {
                string sender = "me@samples.mailgun.org";
                string recipients = MailTo;
                string rawMime =
                    @"X-Priority: 1 (Highest)
                        X-Mailgun-Tag: sample_raw
                        Content-Type: text/plain;charset=UTF-8
                        From: me@samples.mailgun.org
                        To: you@mailgun.info
                        Subject: Hello raw API!
                        
                        This message is sent by Mailgun C# API";

                // Send a simpe text message
                MailgunMessage.SendText(sender, recipients,
                                        "Hello text API!", "Hi!\nI am sending the message using Mailgun C# API");

                // Send a simpe text message and tag it
                var options = new MailgunMessage.Options();
                options.SetHeader(MailgunMessage.MailgunTag, "sample_text");
                MailgunMessage.SendText(sender, recipients,
                                        "Hello text API + tag!",
                                        "Hi!\nI am sending the message using Mailgun C# API and setting the tag",
                                        options);

                // Send a MIME message
                MailgunMessage.SendRaw(sender, recipients, Encoding.UTF8.GetBytes(rawMime));

                // .NET Framework also has System.Net.Mail.MailMessage class which simplifies
                // MIME constriction. MailMessage can be sent by System.Net.Mail.SmtpClient class.
                // Login into Mailgun Control Panel and look for your SMTP server address, user and password.
                if (Request.IsAjaxRequest())
                    return new JsonResult() { Data = new { status = "done" } };

                return Redirect(returnUrl);
            }
            catch (MailgunException ex)
            {
                Console.WriteLine(ex.StackTrace);
                return RedirectToAction("Error");
            }
            catch (Exception)
            {
                if (Request.IsAjaxRequest()) return new JsonResult { Data = new { status = "error" } };
                return RedirectToAction("Error");
            }

        }
    }
}

using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MailgunClient;
using StugService.Web.Models;

namespace StugService.Web.Controllers
{
    public class ContactFormController : Controller
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

//    {
        //        string mailTo = MailTo;
        //        string host = "okänd";
        //        if (Request.Url != null)
        //        {
        //            host = Request.Url.Host;
        //            mailTo = Session[host] + "";
        //        }

        //        var mailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        //        if (mailRegex.Match(model.Mail).Success)
        //            ModelState.AddModelError("mail", "Du måste fylla i en korrekt mejl-adress!");

        //        var fromMail = new MailAddress(model.Mail);
        //        var message = new MailMessage();
        //        var toMail = new MailAddress(mailTo);

        //        message.To.Add(toMail);
        //        message.From = fromMail;

        //        message.Subject = "Kundfråga från webben!";
        //        message.SubjectEncoding = Encoding.UTF8;
        //        message.IsBodyHtml = true;

        //        var body = new StringBuilder();
        //        body.Append(model.Text);
        //        body.Append("<br /><br />");
        //        body.Append(model.Name);

        //        if (!string.IsNullOrEmpty(model.Phone))
        //            body.Append("<br />Telefon: " + model.Phone);

        //        message.Body = body.ToString();
        //        message.BodyEncoding = Encoding.UTF8;

        //        var smtp = new SmtpClient();
        //        smtp.Send(message);


 
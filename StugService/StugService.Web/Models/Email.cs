using System;
using System.Text;
using MailgunClient;

namespace StugService.Web.Models
{
    public class Email
    {
        public void Send(Contact contact)
        {
            Mailgun.Init("key-37i8n53sxysaq5gub3");

            try
            {
                string sender = contact.From;
                string recipients = "christian@wigren.se";
                string rawMime =
@"X-Priority: 1 (Highest)
X-Mailgun-Tag: sample_raw
Content-Type: text/plain;charset=UTF-8
From: " + contact.From +
"To: " + recipients +
@"Subject: Mejl från Stugservice

This message is sent by Mailgun C# API";

                // Send a simpe text message
                MailgunMessage.SendText(sender, recipients,
                    "Hello text API!", "Hi!\nI am sending the message using Mailgun C# API");

                // Send a simpe text message and tag it
                var options = new MailgunMessage.Options();
                options.SetHeader(MailgunMessage.MailgunTag, "sample_text");
                MailgunMessage.SendText(sender, recipients,
                                        "Hello text API + tag!", "Hi!\nI am sending the message using Mailgun C# API and setting the tag",
                                        options);

                // Send a MIME message
                MailgunMessage.SendRaw(sender, recipients, Encoding.UTF8.GetBytes(rawMime));

                // .NET Framework also has System.Net.Mail.MailMessage class which simplifies
                // MIME constriction. MailMessage can be sent by System.Net.Mail.SmtpClient class.
                // Login into Mailgun Control Panel and look for your SMTP server address, user and password.

            }
            catch (MailgunException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}

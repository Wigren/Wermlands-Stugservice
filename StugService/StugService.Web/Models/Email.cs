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
                const string recipients = "christian@wigren.se";

                // Send a simpe text message
                MailgunMessage.SendText(sender, recipients,
                    "Mejl från Stugservice.se", contact.Message);
              
            }
            catch (MailgunException ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}

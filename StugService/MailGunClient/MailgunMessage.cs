using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MailgunClient
{
    public class MailgunMessage
    {
        public static string MailgunTag = "X-Mailgun-Tag";

        public class Options
        {
            private readonly Dictionary<string, Dictionary<string, string>> _options;

            public Options()
            {
                _options = new Dictionary<string, Dictionary<string, string>>();
                _options["headers"] = new Dictionary<string, string>();
            }

            public void SetHeader(string header, string value)
            {
                _options["headers"][header] = value;
            }

            public String ToJSON()
            {
                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(_options);
            }
        }

        public static void SendText(string sender, string recipients, string subject, string text)
        {
            SendText(sender, recipients, subject, text, "");
        }


        public static void SendText(string sender, string recipients, string subject, string text, string servername)
        {
            SendText(sender, recipients, subject, text, servername, null);
        }

        public static void SendText(string sender, string recipients, string subject, string text, Options options)
        {
            SendText(sender, recipients, subject, text, "", options);
        }

        /// <summary>
        /// Send plain-text message 
        /// </summary>
        /// <param name="sender">sender specification</param>
        /// <param name="recipients">comma- or semicolon-separated list of recipients specifications</param>
        /// <param name="subject">message subject</param>
        /// <param name="text">message text</param>
        /// <param name="servername">sending server (can be empty, use 'best' server)</param>
        /// <param name="options">sending options (e.g. add headers to message)</param>
        public static void SendText(string sender, string recipients, string subject, string text, string servername, Options options)
        {
            var req = new NameValueCollection
                          {{"sender", sender}, {"recipients", recipients}, {"subject", subject}, {"body", text}};
            if (options != null)
            {
                req.Add("options", options.ToJSON());
            }

            var data = GetWWWFormData(req);
            HttpWebRequest wr = Mailgun.OpenRequest(MessagesUrl("txt", servername), "POST");
            wr.ContentType = "application/x-www-form-urlencoded";
            wr.ContentLength = data.Length;
            using (Stream rs = Mailgun.GetRequestStream(wr))
                rs.Write(data, 0, data.Length);

            using (Mailgun.SendRequest(wr))
            {
            }
        }

        public static void SendRaw(string sender, string recipients, byte[] rawMime)
        {
            SendRaw(sender, recipients, rawMime, "");
        }

        /// <summary>
        /// Send raw mime message 
        /// </summary>
        /// <param name="sender">sender specification</param>
        /// <param name="recipients">comma- or semicolon-separated list of recipient specifications</param>
        /// <param name="rawMime">mime-encoded message body</param>
        /// <param name="servername">sending server (can be empty, use 'best' server)</param>
        public static void SendRaw(string sender, string recipients, byte[] rawMime, string servername)
        {
            HttpWebRequest wr = Mailgun.OpenRequest(MessagesUrl("mime", servername), "POST");
            byte[] req = Encoding.UTF8.GetBytes(string.Format("{0}\n{1}\n\n", sender, recipients));
            wr.ContentLength = req.Length + rawMime.Length;
            wr.ContentType = "text/plain";
            using (Stream rs = Mailgun.GetRequestStream(wr))
            {
                rs.Write(req, 0, req.Length);
                rs.Write(rawMime, 0, rawMime.Length);
            }
            using (Mailgun.SendRequest(wr))
            {
            }
        }

        protected static string MessagesUrl(string format, string servername)
        {
            return Mailgun.ApiUrl + "messages." + format + "?servername=" + servername;
        }

        protected static byte[] GetWWWFormData(NameValueCollection reqParams)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < reqParams.Count; ++i)
            {
                sb.Append(reqParams.GetKey(i));
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(reqParams.Get(i), Encoding.UTF8));
                sb.Append('&');
            }
            return Encoding.ASCII.GetBytes(sb.ToString());
        }
    }
}
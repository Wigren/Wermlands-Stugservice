using System;
using System.Net;
using System.IO;


namespace MailgunClient
{
    /// <summary>
    /// Mailgun.Init() lets you initialize the library. 
    /// You need API Key. You may provide API URL if for some reason it differs from standard.
    /// </summary>
    public class Mailgun
    {
        /// <summary>
        ///  Initialize the library with standard MailGun API URL
        /// </summary>
        /// <param name="apiKey"></param>
        public static void Init(string apiKey)
        {
            Init(apiKey, "https://mailgun.net/api");
        }

        public static void Init(string apiKey, string apiUrl)
        {
            _apiUrl = apiUrl;
            if (!_apiUrl.EndsWith("/"))
                _apiUrl += "/";
            _cc = new CredentialCache();
            Uri url = new Uri(_apiUrl);
            _cc.Remove(url, "Basic");
            _cc.Add(url, "Basic", new NetworkCredential("api_key", apiKey));
        }

        static internal string ApiUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_apiUrl))
                    throw new MailgunException("Call Mailgun.Init() first");
                return _apiUrl;
            }
        }

        static internal HttpWebRequest OpenRequest(string url, string method)
        {
            // Expect: 100-continue fails behind transparent squid proxy
            ServicePointManager.Expect100Continue = false;
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.Method = method;
            // Turn off proxy auto-detection, it causes long delay on first request
            wr.Proxy = null;
            wr.Credentials = _cc;
            return wr;
        }

        static internal HttpWebResponse SendRequest(HttpWebRequest request)
        {
            try
            {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                throw MailgunException.Wrap(ex);
            }
        }

        static internal Stream GetRequestStream(HttpWebRequest request)
        {
            try
            {
                return request.GetRequestStream();
            }
            catch (WebException ex)
            {
                throw MailgunException.Wrap(ex);
            }
        }

        private static CredentialCache _cc;
        private static string _apiUrl;
    }
}

using System.IO;
using System.Net;
using System.Xml;

namespace MailgunClient
{
    /// <summary>
    /// Mailbox captures all mail arriving to it's address.
    /// Email will be stored on the server and can be later accessed via IMAP or POP3                                                                                   
    /// protocols.                                                                                                                                                
    ///                                                                                                                                                           
    /// Mailbox has several properties:                                                                                                                           
    ///                                                                                                                                                           
    /// alex@gmail.com                                                                                                                                            
    ///  ^      ^                                                                                                                                                 
    ///  |      |                                                                                                                                                 
    /// user    domain                                                                                                                                            
    ///                                                                                                                                                           
    /// and a password                                                                                                                                            
    ///                                                                                                                                                           
    /// user and domain can not be changed for an existing mailbox.   
    /// </summary>
    public class Mailbox : MailgunResourceUpsertable<Mailbox>
    {
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Size
        {
            set { _size = value; }
            get { return _size; }
        }

        private string _user, _domain, _password, _size;

        public Mailbox()
        {
        }

        /// <summary>
        /// Construct the Mailbox.
        /// </summary>
        /// <param name="user">
        /// </param>
        /// <param name="domain">
        /// </param>
        /// <param name="password">
        /// </param>
        public Mailbox(string user, string domain, string password)
        {
            User = user;
            Domain = domain;
            Password = password;
        }

        protected override bool onReadProperty(string propName, object propVal)
        {
            if (base.onReadProperty(propName, propVal))
                return true;
            switch (propName)
            {
                case "user":
                    User = (string)propVal;
                    return true;
                case "domain":
                    Domain = (string)propVal;
                    return true;
                case "size":
                    Size = (string)propVal;
                    return true;
                default:
                    return false;
            }
        }

        protected override void writeInnerXml(XmlWriter xw)
        {
            base.writeInnerXml(xw);
            writeARProperty(xw, "user", User);
            writeARProperty(xw, "domain", Domain);
            writeARProperty(xw, "password", Password);
        }

        public override string ToString()
        {
            return string.Format("Mailbox({0}@{1} {2})", User, Domain, Size);
        }

        internal override ResourceInfo getResourceInfo()
        {
            return _resInfo;
        }

        // our target is C# 2.0, don't use "structure initialization syntax"
        private static ResourceInfo _resInfo = new ResourceInfo("mailboxes", "mailbox");

        public static void UpsertFromCsv(byte[] mailboxes)
        {
            HttpWebRequest wr = Mailgun.OpenRequest(Mailgun.ApiUrl + "mailboxes.txt", "POST");
            wr.ContentLength = mailboxes.Length;
            wr.ContentType = "text/plain";
            using (Stream rs = Mailgun.GetRequestStream(wr))
            {
                rs.Write(mailboxes, 0, mailboxes.Length);
            }
            using (Mailgun.SendRequest(wr))
            {
            }
        }
    }
}
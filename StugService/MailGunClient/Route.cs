using System.Xml;

namespace MailgunClient
{
    /// <summary>
    /// Route represents the basic rule: the message for particular recipient R is forwarded to destination/callback D.
    /// Route has 2 properties: pattern and destination. The pair (pattern, destination) must be unique.
    /// </summary>
    public class Route : MailgunResourceUpsertable<Route>
    {
        public string Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }
        private string _pattern, _destination;

        public Route()
        {
        }

        /// <summary>
        /// Construct the route.
        /// </summary>
        /// <param name="pattern">
        /// The pattern for matching the recipient.
        /// There are 4 types of patterns:
        /// 1. '*' - match all.
        /// 2. exact string match (foo@bar.com)
        /// 3. a domain pattern, i.e. a string like "*@example.com" - matches all emails going to example.com
        /// 4. a regular expression
        /// </param>
        /// <param name="destination">
        /// 1 An email address.
        /// 2 HTTP/HTTPS URL. A message will be HTTP POSTed there.
        /// </param>
        public Route(string pattern, string destination)
        {
            Pattern = pattern;
            Destination = destination;
        }

        protected override bool onReadProperty(string propName, object propVal)
        {
            if (base.onReadProperty(propName, propVal))
                return true;
            switch (propName)
            {
                case "pattern":
                    Pattern = (string)propVal;
                    return true;
                case "destination":
                    Destination = (string)propVal;
                    return true;
                default:
                    return false;
            }
        }

        protected override void writeInnerXml(XmlWriter xw)
        {
            base.writeInnerXml(xw);
            writeARProperty(xw, "pattern", Pattern);
            writeARProperty(xw, "destination", Destination);
        }

        public override string ToString()
        {
            return string.Format("Route({0}, {1}, {2})", Pattern, Destination, Id);
        }

        internal override ResourceInfo getResourceInfo()
        {
            return _resInfo;
        }

        // our target is C# 2.0, don't use "structure initialization syntax"
        private static ResourceInfo _resInfo = new ResourceInfo("routes", "route");
    }
}
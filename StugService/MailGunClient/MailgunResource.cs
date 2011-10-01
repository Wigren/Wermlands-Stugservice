using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace MailgunClient
{
    /// <summary>
    /// Base class providing basic ActiveResource functionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MailgunResource<T> where T : MailgunResource<T>, new()
    {
        /// <summary>
        /// Resource ID.
        /// A resource load the ID in Create/Save/Find operations.
        /// </summary>
        public string Id
        {
            // our target is C# 2.0, don't use automatic properties
            get { return _id; }
            set { _id = value; }
        }
        private string _id;

        /// <summary>
        /// Create new resource.
        /// Throw if "same resorce" already exists. The meaning of "same" depends. 
        /// For example, "same Route exists" if there are route with same pattern and destination.
        /// </summary>
        public void Create()
        {
            update(collectionUrl, "POST");
        }

        /// <summary>
        /// Save modifications or create new resource.
        /// </summary>
        public void Save()
        {
            if (string.IsNullOrEmpty(Id))
                Create();
            else
                update(resourceUrl, "PUT");
        }

        /// <summary>
        /// Delete the resource. 
        /// No error if the resource with given ID does not exist (already deleted).
        /// </summary>
        public void Delete()
        {
            using (Mailgun.SendRequest(Mailgun.OpenRequest(resourceUrl, "DELETE")))
            {
            }
        }

        /// <summary>
        /// Find all resources.
        /// </summary>
        /// <returns></returns>
        public static List<T> Find()
        {
            string collectionUrl = (new T()).collectionUrl;
            // dirty but simple
            HttpWebRequest wr = Mailgun.OpenRequest(collectionUrl, "GET");
            using (HttpWebResponse r = Mailgun.SendRequest(wr))
            {
                if (isResponseXml(r))
                {
                    using (XmlReader xr = XmlReader.Create(r.GetResponseStream()))
                        return readList(xr);
                }
            }
            return new List<T>();
        }

        protected void update(string url, string method)
        {
            update(url, method, true);
        }

        protected void update(string url, string method, bool readUpdatedResource)
        {
            HttpWebRequest wr = Mailgun.OpenRequest(url, method);
            wr.ContentType = "text/xml";
            using (Stream rs = Mailgun.GetRequestStream(wr))
                writeXml(rs);

            using (HttpWebResponse r = Mailgun.SendRequest(wr))
            {
                if (readUpdatedResource && isResponseXml(r))
                {
                    using (XmlReader xr = XmlReader.Create(r.GetResponseStream()))
                        readThis(xr);
                }
            }
        }

        protected static bool isResponseXml(HttpWebResponse r)
        {
            return r.ContentLength > 0 && r.ContentType.StartsWith("text/xml");
        }

        /// <summary>
        /// Read the resource.
        /// Precondition: XmlReader position is on resource elementName tag.
        /// Postcondition: XmlReader position is after elementName closing tag.
        /// </summary>
        /// <param name="xr"></param>
        protected virtual void readThis(XmlReader xr)
        {
            xr.ReadStartElement(elementName);
            while (xr.IsStartElement())
            {
                string propName = xr.LocalName;
                object propVal = null;
                if (!xr.IsEmptyElement)
                {
                    xr.ReadStartElement(xr.LocalName);
                    if (string.IsNullOrEmpty(xr.GetAttribute("nil")))
                        propVal = xr.ReadContentAs(mapARType(xr.GetAttribute("type")), null);
                    xr.ReadEndElement();
                }
                else
                    xr.Read();
                onReadProperty(propName, propVal);
            }
            xr.ReadEndElement();
        }

        /// <summary>
        /// Read list of resources
        /// Precondition: XmlReader position before/on collection tag - any tag having type="array" attribute.
        /// Postcondition:XmlReader position after collection closing tag.
        /// </summary>
        /// <param name="xr"></param>
        /// <returns></returns>
        protected static List<T> readList(XmlReader xr)
        {
            // find <collectionName type="array">
            while (!xr.IsStartElement() || xr.GetAttribute("type") != "array")
                xr.Read();

            List<T> res = new List<T>();
            // for each nested element, readThis()
            if (!xr.IsEmptyElement)
            {
                xr.ReadStartElement(xr.LocalName);
                while (xr.IsStartElement())
                {
                    T resource = new T();
                    resource.readThis(xr);
                    res.Add(resource);
                }
                xr.ReadEndElement();
            }
            else
                xr.Read();
            return res;
        }

        /// <summary>
        /// Write resource as XML, including xml declaration. Encoding is UTF-8.
        /// </summary>
        /// <param name="output"></param>
        protected virtual void writeXml(Stream output)
        {
            XmlWriterSettings xs = new XmlWriterSettings();
            xs.Encoding = Encoding.UTF8;
            using (XmlWriter xw = XmlWriter.Create(output, xs))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement(elementName);
                writeInnerXml(xw);
                xw.WriteEndElement();
            }
        }

        /// <summary>
        /// When overriden, write resource inner content 
        /// (anything between resource opening and closing tags)
        /// </summary>
        /// <param name="xw"></param>
        protected virtual void writeInnerXml(XmlWriter xw)
        {
            // new resource must not write ID
            if (!string.IsNullOrEmpty(Id))
                xw.WriteElementString("id", Id);
        }

        /// <summary>
        /// Write property value.
        /// Currently supports only integers and strings (and null value).
        //
        /// Companion function is mapARType().
        /// </summary>
        protected void writeARProperty(XmlWriter xw, string propName, object propVal)
        {
            //Empty   0
            //Object  1
            //DBNull  2
            //Boolean 3
            //Char    4
            //SByte   5
            //Byte    6
            //Int16   7
            //UInt16  8
            //Int32   9
            //UInt32  10
            //Int64   11
            //UInt64  12
            //Single  13
            //Double  14
            //Decimal 15
            //DateTime 16
            //String  18
            xw.WriteStartElement(propName);
            if (propVal != null)
            {
                int typecode = (int)Type.GetTypeCode(propVal.GetType());
                if (5 <= typecode && typecode <= 12)
                    xw.WriteAttributeString("type", "integer");
                // next line supports int/string only
                xw.WriteString(propVal.ToString());
            }
            else
                xw.WriteAttributeString("nil", "true");
            xw.WriteEndElement();
        }

        /// <summary>
        /// Map ActiveResource type to clr-type.
        /// Currently supports integers and strings.
        /// 
        /// Companion function is writeARProperty().
        /// </summary>
        /// <param name="activeResourceType"></param>
        /// <returns></returns>
        protected static Type mapARType(string activeResourceType)
        {
            switch (activeResourceType)
            {
                case "integer":
                    return typeof(int);
                default:
                    return typeof(string);
            }
        }


        /// <summary>
        /// Override and assign property value.
        /// </summary>
        /// <param name="propName">Name of the property as seen in XML</param>
        /// <param name="propVal">Property value, casted to approproate CLR Type</param>
        /// <returns></returns>
        protected virtual bool onReadProperty(string propName, object propVal)
        {
            if (propName == "id")
            {
                Id = (string)propVal;
                return true;
            }
            return false;
        }

        internal abstract ResourceInfo getResourceInfo();

        protected string resourceUrl
        {
            get { return Mailgun.ApiUrl + collectionName + "/" + Id + ".xml"; }
        }

        protected string collectionUrl
        {
            get { return Mailgun.ApiUrl + collectionName + ".xml"; }
        }

        protected string collectionName
        {
            get { return getResourceInfo().collectionName; }
        }

        protected string elementName
        {
            get { return getResourceInfo().elementName; }
        }
    }
}
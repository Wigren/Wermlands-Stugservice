namespace MailgunClient
{
    internal struct ResourceInfo
    {
        public ResourceInfo(string collectionName, string elementName)
        {
            this.collectionName = collectionName;
            this.elementName = elementName;
        }
        public string collectionName;
        public string elementName;
    }
}
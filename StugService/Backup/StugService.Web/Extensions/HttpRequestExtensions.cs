using System.Web;
using StugService.Web.Common;

namespace StugService.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Lang CurrentLang(this HttpRequestBase request)
        {
            if (request.Url != null)
            {
                var pathAndQuery = request.Url.PathAndQuery;

                if (pathAndQuery.Contains("/En/") || pathAndQuery.EndsWith("/En"))
                    return Lang.En;
                if (pathAndQuery.Contains("/Nl/") || pathAndQuery.EndsWith("/Nl"))
                    return Lang.Nl;
                if (pathAndQuery.Contains("/De/") || pathAndQuery.EndsWith("/De"))
                    return Lang.De;
            }

            return Lang.Sv;
        }

        public static bool IsStartPage(this HttpRequestBase request)
        {
            if (request.Url != null)
            {
                var pathAndQuery = request.Url.PathAndQuery;

                if (pathAndQuery == "/"
                    || pathAndQuery.EndsWith("/En/")
                    || pathAndQuery.EndsWith("/En")
                    || pathAndQuery.EndsWith("/Sv/")
                    || pathAndQuery.EndsWith("/Sv")
                    || pathAndQuery.Contains("/Nl/")
                    || pathAndQuery.EndsWith("/Nl")
                    || pathAndQuery.Contains("/De/")
                    || pathAndQuery.EndsWith("/De"))
                    return true;
            }
            return false;
        }
    }
}
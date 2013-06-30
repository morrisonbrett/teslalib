using System;
using System.Net;


namespace TeslaLib
{
    class TeslaWebClient : WebClient
    {
        public CookieContainer Cookies = new CookieContainer();

        public TeslaWebClient()
        {
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = Cookies;
            return request;
        }

    }
}

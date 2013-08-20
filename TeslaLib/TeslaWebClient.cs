using System;
using System.Net;


namespace TeslaLib
{
    class TeslaWebClient : WebClient
    {
        public CookieContainer Cookies = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.CookieContainer = Cookies;
            return request;
        }
    }
}

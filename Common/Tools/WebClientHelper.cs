using System;
using System.Net;

namespace Common.Tools
{
    public class WebClientHelper : WebClient
    {
        public int Timeout = 60 * 1000;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = Timeout;
            return w;
        }
    }
}
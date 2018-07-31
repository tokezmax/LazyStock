using System;
using System.Web;

namespace Common.Tools
{
    public class AuthHelper
    {
        public static bool IsPowerAdmin(HttpRequestBase Request)
        {
            if (Request.Headers.Get("PowerAdmin") != Common.Tools.Setting.AppSettings("PowerAdmin"))
                throw new Exception("You are not PowerUser!!!I watch you!!!");
            return true;
        }
    }
}
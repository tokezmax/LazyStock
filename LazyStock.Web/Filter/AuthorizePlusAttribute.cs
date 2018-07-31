using System;
using System.Web.Mvc;

namespace LazyStock.Web.Filter
{
    public class AuthorizePlusAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string token = "";
            try
            {
                token = Convert.ToString(filterContext.HttpContext.Request.Cookies["myLogin"].Value);
            }
            catch (System.NullReferenceException)
            {
                token = "";
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                filterContext.Result = new ViewResult() { ViewName = "Index" };
                //base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                var loginTime = Convert.ToDateTime(filterContext.HttpContext.Application[token]);
                if (loginTime > DateTime.UtcNow)
                {
                    //驗證通過
                }
                else
                {
                    filterContext.Result = new ViewResult() { ViewName = "Index" };
                    //base.HandleUnauthorizedRequest(filterContext);
                }
            }
        }
    }
}
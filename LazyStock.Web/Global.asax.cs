using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LazyStock.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            try
            {
                //_logger.Trace("[Application_Init]start");

                Common.Tools.Setting.ReLoad();
                //Common.DataAccess.Dao.LoadTables();
            }
            catch// (Exception ex)
            {
                //_logger.Trace("[Application_Init]" + ex.Message);
            }
        }
    }
}
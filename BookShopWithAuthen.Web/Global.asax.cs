using BookShopWithAuthen.Service.OtherServices;
using BookShopWithAuthen.Web.App_Start;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookShopWithAuthen.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // configure automapper
            AutoMapperWebConfiguration.Configure();
            // configure automap
            AutofacConfiguration.Run();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            if (Server != null)
            {
                Exception ex = Server.GetLastError();
                LogService.GetInstance.LogException(ex.ToString());
                if (Response.StatusCode == 404)
                {

                    Response.RedirectToRoute("/Error/NotFound");
                }
                else if (Response.StatusCode == 400)
                {
                    Response.RedirectToRoute("/Error/BadRequest");
                }
                else
                {
                    Response.Redirect("/Error/Index");
                }

            }
        }


    }
}

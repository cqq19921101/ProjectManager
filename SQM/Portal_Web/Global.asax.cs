using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Lib_Portal_Domain.SharedLibs;
using System.Web.Configuration;
using Telerik.Reporting.Services.WebApi;

namespace Portal_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ReportsControllerConfiguration.RegisterRoutes(GlobalConfiguration.Configuration);

        }

        protected void Session_Start()
        {
            SessionHelper.SetSessionValue(Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SitePurpose),
                WebConfigurationManager.AppSettings.Get(PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.Config_SitePurpose)));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContextWrapper context = new HttpContextWrapper(this.Context);

            if (context.Request.IsAjaxRequest())
            {
                context.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;
using System.Reflection;

namespace WebApplication1.Controllers
{
    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration preservedConfiguration;

        static IReportServiceConfiguration PreservedConfiguration
        {
            get
            {
                if (null == preservedConfiguration)
                {
                    preservedConfiguration = new ReportServiceConfiguration
                    {
                        //HostAppId = "WebApplication2",
                        HostAppId = Assembly.GetCallingAssembly().GetName().Name,
                        Storage = new FileStorage(),
                        ReportResolver = CreateResolver(),
                        // ReportSharingTimeout = 0,
                        // ClientSessionTimeout = 15,
                    };
                }
                return preservedConfiguration;
            }
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = PreservedConfiguration;
        }

        static IReportResolver CreateResolver()
        {
            var appPath = HttpContext.Current.Server.MapPath("~/");
            //var reportsPath = Path.Combine(appPath, @"..\..\..\Report Designer\Examples");
            var reportsPath = Path.Combine(appPath, @"Reports");

            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }
    }
}
using Lib_VMI_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class VMIReportResolverController : Telerik.Reporting.Services.WebApi.ReportsControllerBase
    {
        static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
        new Telerik.Reporting.Services.ReportServiceConfiguration
        {
            HostAppId = Assembly.GetCallingAssembly().GetName().Name,
            ReportResolver = new VMI_ASNReportResolver(),
            Storage = new Telerik.Reporting.Cache.File.FileStorage(),
        };

        public VMIReportResolverController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }

    public class VMIReportBarcodeResolverController : Telerik.Reporting.Services.WebApi.ReportsControllerBase
    {
        static Telerik.Reporting.Services.ReportServiceConfiguration configurationInstance =
        new Telerik.Reporting.Services.ReportServiceConfiguration
        {
            HostAppId = Assembly.GetCallingAssembly().GetName().Name,
            ReportResolver = new VMI_ASNReportBarcodeResolver(),
            Storage = new Telerik.Reporting.Cache.File.FileStorage(),
        };

        public VMIReportBarcodeResolverController()
        {
            this.ReportServiceConfiguration = configurationInstance;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib_VMI_Domain.Model;
using Lib_Portal_Domain.SharedLibs;

namespace Portal_Web.Controllers
{
    public class VMIReportController : Controller
    {
        // GET: VMIReport
        public ActionResult VMIReportForm(string ASN_NUM)
        {
            VMI_Reporting_Helper.VMIReportTemplateModel TM = new VMI_Reporting_Helper.VMIReportTemplateModel();
            TM = VMI_Reporting_Helper.ReportFormResult(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASN_NUM,
                false);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return View(TM);
        }

        public ActionResult VMIASNReportForm(string ASN_NUMs)
        {
            VMI_Reporting_Helper.VMIReportTemplateModel TM = new VMI_Reporting_Helper.VMIReportTemplateModel();

            Dictionary<string, object> para = new Dictionary<string, object>();

            TM.ReportName = ASN_NUMs.Remove(ASN_NUMs.Length - 1);

            return View(TM);
        }

        public ActionResult VMIASNReportFormBarcode(string ASN_NUMs)
        {
            VMI_Reporting_Helper.VMIReportTemplateModel TM = new VMI_Reporting_Helper.VMIReportTemplateModel();

            Dictionary<string, object> para = new Dictionary<string, object>();

            TM.ReportName = ASN_NUMs.Remove(ASN_NUMs.Length - 1);

            return View(TM);
        }
    }
}
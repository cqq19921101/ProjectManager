using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class CPKController : Controller
    {
        // GET: CPK
        [AllowAnonymous]
        [SessionExpire_Service(Order = 1)]
        [SetPortalSessionStopWatch(Order = 60)]
        public ActionResult CPK(string ReportID)
        {
            ViewData["TaskID"] = SQM_Approve_Case_Helper.desDecryptBase64(ReportID);
            string appName = Request.ApplicationPath.ToString();
            string serverName = Request.Url.Authority.ToString();
            string protocol = Regex.Split(Request.Url.AbsoluteUri.ToString(), @"://")[0];
            string urlPre = protocol + "://" + serverName + appName;
            ViewData["appPath2"] = urlPre;
            return View();
        }
        [AllowAnonymous]
        public string  UpdateReport(string TaskID,string Remark)
        {
            string r = SQMCPK_Helper.UpdateReMark(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                 , StringHelper.EmptyOrUnescapedStringViaUrlDecode(TaskID), StringHelper.EmptyOrUnescapedStringViaUrlDecode(Remark)
                 );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
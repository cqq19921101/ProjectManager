using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQEQualityController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQEQuality
        public ActionResult SQEQuality()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQM_Quality_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityInspJSonWithFilter(string SInspCode, String SInspDesc, string ReportSID)
        {
            string r = SQM_QualityInsp_Helper.GetDataToJQGridJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspCode)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspDesc)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(ReportSID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityFileJSonWithFilter(string SearchText, string ReportSID)
        {
            string r = SQM_QualityFile_Helper.GetDataToJQGridJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(ReportSID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
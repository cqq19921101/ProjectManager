using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
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
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQMMailRController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMMailR
        public ActionResult SQMMailR()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryCorpEMAIL(string Email)
        {
            string r = SQMMailRMgmt_Helper.Query_EmailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                Email.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SendMailSQE(string PlantCode, string VendorCode, string Email)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMMailRMgmt_Helper.SendMailSQE(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantCode),
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(VendorCode),
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(Email),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                urlPre,
                localPath
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QuerySBUVendorMail(string ERP_VND)
        {
            string r = SQMMailRMgmt_Helper.Query_SBUVendorCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ERP_VND.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPlantMail(string PLANT)
        {
            string r = SQMMailRMgmt_Helper.Query_PlantCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PLANT.Trim(), PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

    }
}
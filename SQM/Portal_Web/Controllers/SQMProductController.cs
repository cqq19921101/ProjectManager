using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.PortalLogicsForGeneralControllerAction;
using Portal_Web.SupportClasses;
using Lib_SQM_Domain.Modal;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]

    public class SQMProductController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: /SQMProduct/
        public ActionResult SQMProduct()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadProduct(string SearchText, string MemberType, string BasicInfoGUID)
        {
            string r = SystemMgmt_Pro_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), StringHelper.EmptyOrUnescapedStringViaUrlDecode(BasicInfoGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreatePro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeletePro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQMInspController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMInsp
        public ActionResult SQMInsp()
        {
            return View();
        }

        #region InspCode
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadInsp(string SearchText)
        {
            string r = SQMInsp_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region InspMap

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadInspMap(string SearchText, String SID)
        {
            string r = SQMInspMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , SID == null ? "" : SID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
              );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}
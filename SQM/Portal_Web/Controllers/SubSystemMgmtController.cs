using Lib_Portal_Domain.Model;
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
    public class SubSystemMgmtController : Controller
    {
        #region MemberManagement

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadMemberJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SystemMgmt_Member_Helper.GetDataToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), MemberType);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetInternalUserInfoByAccountID(string AccountID)
        {
            string r = SystemMgmt_Member_Helper.GetInternalMemberInfoJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), AccountID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.SPMDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMember(SystemMgmt_Member DataItem)
        {
            string r = SystemMgmt_Member_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMember(SystemMgmt_Member DataItem)
        {
            string r = SystemMgmt_Member_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMember(SystemMgmt_Member DataItem)
        {
            string r = SystemMgmt_Member_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateMemberRoles(string MemberGUID, List<string> MemberRoles)
        {
            string r = SQM_SystemMgmt_Member_Helper.UpdateMemberRoles(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), MemberGUID, MemberRoles);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region GetMemberByMember
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SubMemberManagement()
        {
            return View();
        }
        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public string GetMemberRolesByMember(String MemberGUID)
        {
            string SubMemberGUID = MemberGUID;
            string r = SysMember_Helper.GetMemberRoleByMemberListJSon(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                , SubMemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}
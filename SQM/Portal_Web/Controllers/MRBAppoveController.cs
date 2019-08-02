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
    public class MRBAppoveController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult MRBAppove()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadJSonWithFilter(string SearchText)
        {
            string r = MRBAppove_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Create(MRBAppove DataItem)
        {
            string r = MRBAppove_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Edit(MRBAppove DataItem)
        {
            string r = MRBAppove_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Delete(MRBAppove DataItem)
        {
            string r = MRBAppove_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadMapJSonWithFilter(string SearchText)
        {
            string r = MRBAppoveMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMap(MRBAppoveMap DataItem)
        {
            string r = MRBAppoveMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMap(MRBAppoveMap DataItem)
        {
            string r = MRBAppoveMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMap(MRBAppoveMap DataItem)
        {
            string r = MRBAppoveMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
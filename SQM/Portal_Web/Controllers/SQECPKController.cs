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
    public class SQECPKController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQECPK
        public ActionResult SQECPK()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCPK(string SearchText)
        {
            string r = SQE_CPK_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCPK(SQE_CPK DataItem)
        {
            string r = SQE_CPK_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem,
                 PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCPK(SQE_CPK DataItem)
        {
            
            string r = SQE_CPK_Helper.EditDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCPK(SQE_CPK DataItem)
        {
            string r = SQE_CPK_Helper.DeleteDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
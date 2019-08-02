using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Model;

namespace Portal_Web.Controllers
{
    public class SQMProcessController : Controller
    {
        // GET: SQMProcess
        public ActionResult ProcessInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMProduct1JSonWithFilter(string SearchText, string MemberType, string BasicInfoGUID)
        {
            string r = SQMProcess_ProcessInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), BasicInfoGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetProcessCategoryList()
        {
            string r = SQMProcess_ProcessInfo_Helper.GetProcessCategoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMember(SQMProcess_ProcessInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMProcess_ProcessInfo_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMember(SQMProcess_ProcessInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMProcess_ProcessInfo_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMember(SQMProcess_ProcessInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMProcess_ProcessInfo_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,"","");
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapVendorCode()
        {
            string r = SQMProcess_ProcessInfo_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
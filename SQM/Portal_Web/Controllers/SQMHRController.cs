using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using Lib_Portal_Domain.SharedLibs;

namespace Portal_Web.Controllers
{
    public class SQMHRController : Controller
    {
        // GET: SQMHR
        public ActionResult HRInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMProduct1JSonWithFilter(string SearchText, string BasicInfoGUID, string MemberType)
        {
            string VendorCode = GetMapVendorCode();
            string r = SQMHR_HRInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), VendorCode, BasicInfoGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCommodityList()
        {
            string r = SQMHR_HRInfo_Helper.GetCommodityList(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMember(SQMHR_HRInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMHR_HRInfo_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, "", "");
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetHRCategoryList()
        {
            string r = SQMHR_HRInfo_Helper.GetHRCategoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMember(SQMHR_HRInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMHR_HRInfo_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapVendorCode()
        {
            string r = SQMHR_HRInfo_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMember(SQMHR_HRInfo DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMHR_HRInfo_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTotalData(string BasicInfoGUID)
        {
            string VendorCode = GetMapVendorCode();
            string r = SQMHR_HRInfo_Helper.GetTotalData(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext), VendorCode, BasicInfoGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
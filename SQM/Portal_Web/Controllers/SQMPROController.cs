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
    public class SQMPROController : Controller
    {
        // GET: SQMPRO
        public ActionResult SQMProduct1()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMProduct1JSonWithFilter(string SearchText, string MemberType)
        {
            string VendorCode = GetMapVendorCode();
            string r = SQMPRO_SQMProduct1_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), VendorCode);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCommodityList()
        {
            string r = SQMPRO_SQMProduct1_Helper.GetCommodityList(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMember(SQMPRO_SQMProduct1 DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMPRO_SQMProduct1_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, "", "");
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetHRCategoryList()
        {
            string r = SQMPRO_SQMProduct1_Helper.GetHRCategoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMember(SQMPRO_SQMProduct1 DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMPRO_SQMProduct1_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapVendorCode()
        {
            string r = SQMPRO_SQMProduct1_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMember(SQMPRO_SQMProduct1 DataItem)
        {
            DataItem.VendorCode = GetMapVendorCode();
            string r = SQMPRO_SQMProduct1_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTotalData()
        {
            string VendorCode = GetMapVendorCode();
            string r = SQMPRO_SQMProduct1_Helper.GetTotalData(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext), VendorCode);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
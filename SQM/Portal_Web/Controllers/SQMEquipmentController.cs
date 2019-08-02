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
    public class SQMEquipmentController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMEquipment
        public ActionResult SQMEquipment()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetEquipmentSpecialType(String MainID)
        {
            string r = SQM_EquipmentMgmt_Helper.GetEquipmentSpecialType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),MainID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetEquipmentType()
        {
            string r = SQM_EquipmentMgmt_Helper.GetEquipmentType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadEquipmentJSonWithFilter(string EquipmentType, string EquipmentSpecialType,string BasicInfoGUID, string MemberType)
        {
            string r = SQM_EquipmentMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentSpecialType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(BasicInfoGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string getModel(string EquipmentType, string EquipmentSpecialType, string MemberType)
        {
            string r = SQM_EquipmentMgmt_Helper.GetModel(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentSpecialType));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
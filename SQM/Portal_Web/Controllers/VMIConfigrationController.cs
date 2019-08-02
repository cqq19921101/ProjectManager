using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Portal_Web.SupportClasses;
using Lib_VMI_Domain.Model;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class VMIConfigrationController : Controller
    {
        // GET: VMIConfigration
        public ActionResult Index()
        {
            return View();
        }

        #region ASNVendorProfile
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ASNVendorProfile()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNVendorListJsonWithFilter(string CorpVendorCode, string SBUVendorCode)
        {
            string r = VMI_Configration_Helper.Query_ASNVendorList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                CorpVendorCode.Trim(),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNVendorProfileJsonWithFilter(string CorpVendorCode, string SBUVendorCode)
        {
            string r = VMI_Configration_Helper.Query_ASNVendorProfile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                CorpVendorCode.Trim(),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryCustomerCodeAllPlantJsonWithFilter(string SBUVendorCode)
        {
            string r = VMI_Configration_Helper.Query_CustomerCodeForAllPlant(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryShipFromJsonWithFilter(string SBUVendorCode)
        {
            string r = VMI_Configration_Helper.Query_ShipFrom(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateASNVendorProfile(VMI_ConfigQueryASNVendorProfile Header, string DetailCC, string DetailSF, string DetailFI)
        {
            string r = VMI_Configration_Helper.UpdateASNVendorProfile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Header,
                DetailCC,
                DetailSF,
                DetailFI);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryForwarderJsonWithFilter(string SBUVendorCode)
        {
            string r = VMI_Configration_Helper.Query_Forwarder(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                SBUVendorCode.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Plant Forwarder Info
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult PlantForwarderInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPlantForwarderInfoJsonWithFilter(string PLANT, string COMPANY_NAME)
        {
            string r = VMI_Configration_Helper.QueryPlantForwarderInfoJsonWithFilter(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                COMPANY_NAME);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPlantForwarderInfo(VMI_ConfigEditPlantForwarderInfo Item)
        {
            string r = VMI_Configration_Helper.EditPlantForwarderInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Plant Receive Info
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult PlantReceiveInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPlantReceiveInfoJsonWithFilter(string PLANT)
        {
            string r = VMI_Configration_Helper.QueryPlantReceiveInfoJsonWithFilter(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPlantReceiveInfo(VMI_ConfigEditPlantReceiveInfo Item)
        {
            string r = VMI_Configration_Helper.EditPlantReceiveInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Plant Customs Broker Info
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult PlantCustomsBrokerInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPlantCBInfoJsonWithFilter(string PLANT)
        {
            string r = VMI_Configration_Helper.QueryPlantCustomsBrokerInfoJsonWithFilter(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPlantCustomsBrokerInfo(VMI_ConfigEditPlantCustomsBrokerInfo Item)
        {
            string r = VMI_Configration_Helper.EditPlantCustomsBrokerInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Group Mgmt
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult GroupMgmt()
        {
            return View();
        }
        
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadDepartment()
        {
            string r = VMI_Configration_Helper.LoadDepartment(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string AddGroup(string PARENT_GROUP_ID, string GROUP_NAME, string GROUP_TYPE)
        {
            string r = VMI_Configration_Helper.AddGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), 
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PARENT_GROUP_ID,
                GROUP_NAME,
                GROUP_TYPE);

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditGroup(string PARENT_GROUP_ID, string GROUP_NAME, string GROUP_TYPE, string GROUP_ID)
        {
            string r = VMI_Configration_Helper.EditGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PARENT_GROUP_ID,
                GROUP_NAME,
                GROUP_TYPE,
                GROUP_ID);

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string MoveGroup(string GROUP_ID, string PARENT_GROUP_ID, string NEW_POSITION)
        {
            string r = VMI_Configration_Helper.MoveGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                GROUP_ID,
                PARENT_GROUP_ID,
                NEW_POSITION);

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteGroup(string GROUP_ID, string GROUP_TYPE)
        {
            string r = VMI_Configration_Helper.DeleteGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                GROUP_ID,
                GROUP_TYPE);

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion

        #region Buyer Group
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult BuyerGroup()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryBuyerGroupJsonWithFilter(BuyerGroup item)
        {
            string r = VMI_Configration_Helper.QueryBuyerGroupJsonWithFilter(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditBuyerGroup(BuyerGroup item)
        {
            string r = VMI_Configration_Helper.EditBuyerGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteBuyerGroup(BuyerGroup item)
        {
            string r = VMI_Configration_Helper.DeleteBuyerGroup(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}
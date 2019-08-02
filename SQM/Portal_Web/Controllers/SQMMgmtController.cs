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
using Lib_SQM_Domain.Model;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQMMgmtController : Controller
    {
        //
        // GET: /SystemMgmt/
        #region SubFuncManagement
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SubFuncManagement()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSubFuncJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SystemMgmt_SubFunc_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateSubFunc(SystemMgmt_SubFunc DataItem)
        {
            string r = SystemMgmt_SubFunc_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditSubFunc(SystemMgmt_SubFunc DataItem)
        {
            string r = SystemMgmt_SubFunc_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteSubFunc(SystemMgmt_SubFunc DataItem)
        {
            string r = SystemMgmt_SubFunc_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        
        #region MenuSubFuncMgmt
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult MenuSubFuncMgmt()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Page(Order = 1)]
        public ActionResult MenuStructureMgmt(string MenuJSon)
        {
            if (MenuJSon != null)
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                List<MenuItemFromView2> SubmittedMenu = json_serializer.Deserialize<List<MenuItemFromView2>>(MenuJSon);

                string r = SystemMgmt_Menu_Helper.UpdateMenu(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SubmittedMenu);
                DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
                if (r == "")
                {
                    ViewBag.SuccessMsg = "Portal menu update successfully.";
                    ViewBag.ErrorMsg = "";
                }
                else
                {
                    ViewBag.SuccessMsg = "";
                    ViewBag.ErrorMsg = "Portal menu update fail: " + r;
                }
            }
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadMenuJSon()
        {
            string r = SystemMgmt_Menu_Helper.LoadMenuJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetFunctionRoles(string RolesString)
        {
            string r = SystemMgmt_Menu_Helper.GetFunctionRoleFullListJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), (RolesString == null) ? "" : RolesString);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSubFuncCount(string FunctionGUID)
        {
            string r = SystemMgmt_Menu_Helper.GetSubFuncCount(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSubFuncsJSonWithFilter(string SearchText)
        {
            string r = SystemMgmt_Menu_Helper.GetDataToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSubFuncsMapJSonWithFilter(string FunctionGUID,string SearchText)
        {
            string r = SystemMgmt_Menu_Helper.GetSubFuncMapToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), FunctionGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveaSubFunc(string FunctionGUID, string SubFuncGUID)
        {
            string r = SystemMgmt_Menu_Helper.RemoveaSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SubFuncGUID, FunctionGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveAllSubFuncs(string FunctionGUID)
        {
            string r = SystemMgmt_Menu_Helper.RemoveaSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [SessionExpire_Service(Order = 1)]
        public string AddaSubFunc(string FunctionGUID, string SubFuncGUID)
        {
            string r = SystemMgmt_Menu_Helper.AddaSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID, SubFuncGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveaRoleSubFunc(string FunctionGUID, string RoleGUID, string SubFuncGUID)
        {
            string r = SystemMgmt_Menu_Helper.RemoveaRoleSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID, RoleGUID,  SubFuncGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveAllRoleSubFuncs(string FunctionGUID, string RoleGUID)
        {
            string r = SystemMgmt_Menu_Helper.RemoveaRoleSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID ,RoleGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [SessionExpire_Service(Order = 1)]
        public string AddaRoleSubFunc(string FunctionGUID, string RoleGUID, string SubFuncGUID)
        {
            string r = SystemMgmt_Menu_Helper.AddaRoleSubFunc(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID, RoleGUID, SubFuncGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleSubFuncCount(string FunctionGUID, string RoleGUID)
        {
            string r = SystemMgmt_Menu_Helper.GetRoleSubFuncCount(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), FunctionGUID, RoleGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string LoadRoleSubFuncsJSonWithFilter(string SearchText)
        //{
        //    string r = SystemMgmt_Menu_Helper.GetDataToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleSubFuncsMapJSonWithFilter(string FunctionGUID, string RoleGUID, string SearchText)
        {
            string r = SystemMgmt_Menu_Helper.GetRoleSubFuncMapToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), FunctionGUID, RoleGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region VendorList 
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult VendorList()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadVendorJSonWithFilter(string SearchText)
        {
            string r = SQM_RegisterMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region CreateSQMVendorAccount
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult VendorAccountMgmt()
        {
            return View();
        }
        #endregion
    }
}

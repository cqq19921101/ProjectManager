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
using Lib_Portal_Domain.Model;
using Lib_VMI_Domain.Model;
using Lib_SQM_Domain.Model;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SystemMgmtController : Controller
    {
        //
        // GET: /SystemMgmt/

        #region MemberManagement
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult MemberManagement()
        {
            return View();
        }

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

        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public string GetMemberRoles(string MemberGUID)
        {
            string r = SystemMgmt_Member_Helper.GetMemberRoleFullListJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateMemberRoles(string MemberGUID, List<string> MemberRoles)
        {
            string r = SystemMgmt_Member_Helper.UpdateMemberRoles(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), MemberGUID, MemberRoles);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region PortalUserProfile
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult PortalUserProfile()
        {
            return View(PortalUserProfileHelper.GetPortalUser(HttpContext));
        }
        #endregion

        #region RunAs
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult RunAs()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Page(Order = 1)]
        public ActionResult RunAs(string RunAsMemberGUID)
        {
            PortalUserProfile u = PortalUserProfileHelper.GetPortalUserProfile(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RunAsMemberGUID);
            if (u == null)
            {
                ViewBag.Message = "Selected user has no permission, Run As failed.";
                return View();
            }
            else
            {
                PortalUserProfileHelper.GetPortalUser(HttpContext).RunAs(u);

                PortalController_SystemMgmt pc_SystemMgmt = new PortalController_SystemMgmt();

                bool bViaSSL = true;
                if (HttpContext.Request.ServerVariables["HTTPS"].ToString() == "off")
                    bViaSSL = false;

                pc_SystemMgmt.RunAs_HttpPost(Session, RunAsMemberGUID, bViaSSL, Url);

                #region Generate menu after Run As...
                //Add for generate Menu...etc.
                //Trevor: 09/07/2015

                //Step 2: Locale
                //string sLocaleString = "en-US";
                //if (Request.UserLanguages.Length > 0)
                //{
                //    switch (Request.UserLanguages[0])
                //    {
                //        case "zh-CN":
                //        case "zh-TW":
                //            sLocaleString = Request.UserLanguages[0];
                //            break;
                //        default:
                //            break;
                //    }
                //}
                string sLocaleString = "en-US";
                if (Request.UserLanguages.Length > 0)
                {
                    string UserLanguages = Request.UserLanguages[0].ToUpper();

                    if (UserLanguages == "ZH" || UserLanguages == "ZH-TW" || UserLanguages.IndexOf("ZH-HANT") != -1 || UserLanguages == "ZH-CHT" || UserLanguages == "ZH-HK" || UserLanguages == "ZH-MO")
                    {
                        sLocaleString = "zh-TW";
                    }
                    else if (UserLanguages == "ZH-CHS" || UserLanguages.IndexOf("ZH-HANS") != -1 || UserLanguages == "ZH-CN" || UserLanguages == "ZH-SG")
                    {
                        sLocaleString = "zh-CN";
                    }
                    else
                    {
                        sLocaleString = "en-US";
                    }
                }
                //Step 6: Generate Menu (based on RunAs User)
                SessionHelper.SetSessionValue(Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_MenuForRunAsUser),
                    PortalUserProfileHelper.GenerateMemberMenu(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    sLocaleString, RunAsMemberGUID, "Portal-Main-Menu", bViaSSL, new UrlHelper(Request.RequestContext)));
                DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
                // SOSA Url
                VMI_Common_Helper.AddSOSAInMenu(u.AccountID, Session, Request, sLocaleString);
                
                //Step 7: Generate OBP Menu
                //SessionHelper.SetSessionValue(filterContext.HttpContext.Session, "OBPMenuString", OBPv2MenuHelper.GenerateOBPv2MenuJson(cnPortal, SessionHelper.GetSessionStringValue(filterContext.HttpContext.Session, "AppPath")));

                return RedirectToAction("Index", "Home");
                #endregion
            }
        }
        #endregion

        //#region RunAs_VMI
        //[LogActionAccess]
        //[SessionExpire_Page(Order = 10)]
        //[SetFunctionPathValueForPortal(Order = 20)]
        //public ActionResult RunAs_VMI()
        //{
        //    ViewBag.Message = "";
        //    return View();
        //}

        //[HttpPost]
        //[SessionExpire_Page(Order = 1)]
        //public ActionResult RunAs_VMI(string RunAsAccount)
        //{
        //    bool bRunAsSuccessfully = false;

        //    string sRunAsAccount = StringHelper.EmptyOrUnescapedStringViaUrlDecode(RunAsAccount).Trim();
        //    if (!sRunAsAccount.Contains(" "))
        //    {
        //        string RunAsMemberGUID = PortalUserProfileHelper.GetPortalMemberGUIDByAccountID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), sRunAsAccount);
        //        if (RunAsMemberGUID != "")
        //            if (PortalUserProfileHelper.CheckAccountStatusByAccountID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), sRunAsAccount))
        //            {
        //                PortalUserProfile u = PortalUserProfileHelper.GetPortalUserProfile(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RunAsMemberGUID);
        //                PortalUserProfileHelper.GetPortalUser(HttpContext).RunAs(u);
        //                PortalController_SystemMgmt pc_SystemMgmt = new PortalController_SystemMgmt();
        //                //Step 2: Locale
        //                string sLocaleString = "en-US"; // Default locale "en-US"
        //                string oriLocaleString = SessionHelper.GetSessionStringValue(Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_UserSelectedLocale));

        //                if (oriLocaleString != "")
        //                {
        //                    sLocaleString = oriLocaleString;
        //                }
        //                else if (Request.UserLanguages.Length > 0)
        //                {
        //                    string UserLanguages = Request.UserLanguages[0].ToUpper();

        //                    if (UserLanguages == "ZH" || UserLanguages == "ZH-TW" || UserLanguages.IndexOf("ZH-HANT") != -1 || UserLanguages == "ZH-CHT" || UserLanguages == "ZH-HK" || UserLanguages == "ZH-MO")
        //                    {
        //                        sLocaleString = "zh-TW";
        //                    }
        //                    else if (UserLanguages == "ZH-CHS" || UserLanguages.IndexOf("ZH-HANS") != -1 || UserLanguages == "ZH-CN" || UserLanguages == "ZH-SG")
        //                    {
        //                        sLocaleString = "zh-CN";
        //                    }
        //                    else
        //                    {
        //                        sLocaleString = "en-US";
        //                    }
        //                    //switch (Request.UserLanguages[0])
        //                    //{
        //                    //    case "zh-CN":
        //                    //    case "zh-TW":
        //                    //        sLocaleString = Request.UserLanguages[0];
        //                    //        break;
        //                    //    default:
        //                    //        break;
        //                    //}
        //                }

        //                SessionHelper.SetSessionValue(Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_UserSelectedLocale), sLocaleString);

        //                pc_SystemMgmt.RunAs_VMI_HttpPost(Session, u, (HttpContext.Request.ServerVariables["HTTPS"].ToString() == "off") ? false : true, Url);

        //                bRunAsSuccessfully = true;
        //                return RedirectToAction("Index", "Home");
        //            }
        //        DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    }

        //    if (bRunAsSuccessfully)
        //        return RedirectToAction("Index", "Home");
        //    else
        //    {
        //        ViewBag.Message = "Account is not valid.";
        //        return View();
        //    }
        //}
        //#endregion

        #region RoleManagement
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult RoleManagement()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SystemMgmt_Role_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateRole(SystemMgmt_Role DataItem)
        {
            string r = SystemMgmt_Role_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditRole(SystemMgmt_Role DataItem)
        {
            string r = SystemMgmt_Role_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteRole(SystemMgmt_Role DataItem)
        {
            string r = SystemMgmt_Role_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region RoleMemberAndDelegation
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult RoleMemberAndDelegation()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleAdminJSonWithFilter(string SearchText, string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.GetRoleAdminsToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleAdminCount(string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.GetRoleAdminsCount(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string AddaRoleAdmin(string RoleGUID, string MemberGUID)
        {
            string r = SystemMgmt_Role_Helper.AddaRoleAdmin(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, MemberGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveaRoleAdmin(string RoleGUID, string MemberGUID)
        {
            string r = SystemMgmt_Role_Helper.RemoveaRoleAdmin(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, MemberGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveAllRoleAdmins(string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.RemoveaRoleAdmin(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleMemberJSonWithFilter(string SearchText, string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.GetRoleMembersToJQGridJSon(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoleMemberCount(string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.GetRoleMembersCount(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [SessionExpire_Service(Order = 1)]
        public string AddaRoleMember(string RoleGUID, string MemberGUID)
        {
            string r = SystemMgmt_Role_Helper.AddaRoleMember(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, MemberGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveaRoleMember(string RoleGUID, string MemberGUID)
        {
            string r = SystemMgmt_Role_Helper.RemoveaRoleMember(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID, MemberGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RemoveAllRoleMembers(string RoleGUID)
        {
            string r = SystemMgmt_Role_Helper.RemoveaRoleMember(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), RoleGUID,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region RoleMemberMgmt
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult RoleMemberMgmt()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadDelegatedRoleJSonWithFilter(string SearchText)
        {
            string r = SystemMgmt_Role_Helper.GetDelegatedDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion

        #region MenuStructureMgmt
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult MenuStructureMgmt()
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
                List<MenuItemFromView> SubmittedMenu = json_serializer.Deserialize<List<MenuItemFromView>>(MenuJSon);

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
        #endregion

        #region List Current Logins (remarked now.)
        // Mechanism to be changed, remarked now.
        //[LogActionAccess]
        //[SessionExpire_Page(Order = 10)]
        //[SetFunctionPathValueForPortal(Order = 20)]
        //public ActionResult ListCurrentLogins()
        //{
        //    ViewBag.ActiveUserList = LoginInfoHelper.GetActiveUserList(HttpContext.Application);
        //    return View();
        //}
        #endregion
    }
}

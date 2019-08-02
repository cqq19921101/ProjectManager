using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Lib_Portal_Domain.SharedLibs;
using System.Threading;
using System.Data.SqlClient;
using Lib_Portal_Domain.Model;
using System.Net;
using System.Net.Http;
using Lib_Portal_MUIResources;
using System.Text;
using System.Web.Routing;
using System.Data;
using Lib_Portal_Domain.Abstract;
using Lib_Portal_Domain.Concrete;
using Lib_VMI_Domain.Model;

namespace Portal_Web.SupportClasses
{
    public class SetPortalSessionStopWatch : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
            if (!rd.GetRequiredString("action").Equals("CheckSessionExpired"))
                SessionExpireControlHelper.SetPortalSessionStopWatch(filterContext.HttpContext.Session);
        }
    }

    //Function Path below menu bar in Partal
    public class SetFunctionPathValueForPortal : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionHelper.GetSessionObjectValue(filterContext.HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SessionLoginUserKey)) != null)
                SessionHelper.SetSessionValue(filterContext.HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_FunctionPathString),
                    PortalFunctionPathHelper.BuildFunctionPath(Portal_MUI.ResourceManager.GetString(PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.PortalFunctionPathPrefix) +
                                                                                                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "_" +
                                                                                                    filterContext.ActionDescriptor.ActionName)));
        }
    }

    //Use Identity from Windows Authentication
    #region GetUserProfileViaWindowsAuthentication
    public class GetUserProfileViaWindowsAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool bUserProfileExists = false;

            PortalUserProfile RunAsUser;
            try
            {
                RunAsUser = PortalUserProfileHelper.GetRunAsUserProfile(filterContext.HttpContext);
                bUserProfileExists = true;
            }
            catch { }

            if (!bUserProfileExists)
            {
                string sLoginID = filterContext.HttpContext.User.Identity.Name;
                bool bPassCheck = false;

                int iSep = sLoginID.IndexOf(@"\");
                if (iSep > 0)
                    if (sLoginID.Substring(0, iSep).ToUpper() == "LITEON")
                    {
                        //sLoginID = "12345abcde";
                        sLoginID = sLoginID.Substring(iSep + 1);
                        bPassCheck = true;
                    }


                SqlConnection cnPortal = null;
                string sMemberGUID = "";

                //to be deleted
                //if (bPassCheck)
                //{
                //    cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);
                //    sMemberGUID = this.GetValidInternalMemberGUID(cnPortal, sLoginID);
                //    if (sMemberGUID == "")
                //        bPassCheck = false;
                //}

                //Check for iPower login user id
                if (bPassCheck)
                {
                    cnPortal = DBConnHelper.GetPortalDBConnectionOpen(PortalDBType.PortalDB);

                    ILoginAuth _LoginAuthProvider = new LoginAuth_LiteOnADandPortalMember();
                    sMemberGUID = this.GetValidInternalMemberGUID(cnPortal, filterContext.HttpContext.User.Identity.Name);
                    if (sMemberGUID == "")
                        bPassCheck = false;
                }

                string sLocaleString = "en-US";
                if (filterContext.HttpContext.Request.UserLanguages.Length > 0)
                {
                    string UserLanguages = filterContext.HttpContext.Request.UserLanguages[0].ToUpper();

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

                if (bPassCheck)
                {
                    bool bViaSSL = true;
                    if (filterContext.HttpContext.Request.ServerVariables["HTTPS"].ToString() == "off") bViaSSL = false;

                    //Step 1: AppPath
                    this.InitAppPathInSession(filterContext.HttpContext.Session, filterContext.HttpContext.Request);

                    //Step 2: Locale
                    //string sLocaleString = "en-US";
                    //if (filterContext.HttpContext.Request.UserLanguages.Length > 0)
                    //{
                    //    switch (filterContext.HttpContext.Request.UserLanguages[0])
                    //    {
                    //        case "zh-CN":
                    //        case "zh-TW":
                    //            sLocaleString = filterContext.HttpContext.Request.UserLanguages[0];
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    
                    //Step 3: Get LoginUserProfile
                    PortalUserProfile LoginUserProfile = PortalUserProfileHelper.GetPortalUserProfile(cnPortal, sMemberGUID);

                    //Step 5: Pass authentication
                    PortalUser u = new PortalUser(LoginUserProfile);
                    SessionHelper.SetSessionValue(filterContext.HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SessionLoginUserKey), u);

                    //Step 6: Generate Menu (based on RunAs User)
                    string sRoleIDs = "";
                    foreach (ProfileRole r in u.RunAsUser.Roles)
                        sRoleIDs += ", '" + r.RoleGUID + "'";
                    sRoleIDs = sRoleIDs.Substring(2);
                    //SessionHelper.SetSessionValue(filterContext.HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_MenuForRunAsUser),
                    //    PortalUserProfileHelper.GenerateMemberMenuViaRoleIDList(cnPortal, sLocaleString, sRoleIDs, "Portal-Main-Menu", bViaSSL, ((Controller)filterContext.Controller).Url));
                    SessionHelper.SetSessionValue(filterContext.HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_MenuForRunAsUser),
                        PortalUserProfileHelper.GenerateMemberMenu(cnPortal, sLocaleString, u.RunAsUser.MemberGUID, "Portal-Main-Menu", bViaSSL, new UrlHelper(filterContext.RequestContext)));
                    // SOSA Url
                    VMI_Common_Helper.AddSOSAInMenu(filterContext.HttpContext.User.Identity.Name, filterContext.HttpContext.Session, filterContext.HttpContext.Request, sLocaleString);
                }
                else
                {
                    // Non VMI user but domain user
                    this.InitAppPathInSession(filterContext.HttpContext.Session, filterContext.HttpContext.Request);

                    if (VMI_Common_Helper.AuthNonVMIUser(filterContext.HttpContext.User.Identity.Name, "", false, filterContext.HttpContext.Session))
                    {
                        VMI_Common_Helper.AddSOSAInMenu(filterContext.HttpContext.User.Identity.Name, filterContext.HttpContext.Session, filterContext.HttpContext.Request, sLocaleString);
                        bPassCheck = true;
                    }
                }

                if (cnPortal != null)
                {
                    cnPortal.Close();
                    cnPortal = null;
                }

                if (!bPassCheck)
                {
                    var redirectTarget = new RouteValueDictionary { { "action", "Unauthorized" }, { "controller", "Home" } };
                    filterContext.Result = new RedirectToRouteResult(redirectTarget);

                    return;
                }
            }
        }

        private void InitAppPathInSession(HttpSessionStateBase Session, HttpRequestBase Request)
        {
            if (SessionHelper.GetSessionStringValue(Session, "AppPath") == "")
            {
                string sSrcPath = "";
                //try
                //{
                //    sSrcPath = Request.QueryString["ReturnUrl"];
                //    if (sSrcPath.Substring(sSrcPath.Length - 1, 1) == @"/") sSrcPath = sSrcPath.Substring(0, sSrcPath.Length - 1);
                //}
                //catch
                //{
                //    sSrcPath = Request.ApplicationPath;
                //}
                sSrcPath = Request.ApplicationPath;
                SessionHelper.SetSessionValue(Session, "AppPath", sSrcPath);
            }
        }

        private string GetValidInternalMemberGUID(SqlConnection cnPortal, string loginid)
        {
            return this.GetPortalMemberGUID(cnPortal, loginid, true);
        }

        private string GetPortalMemberGUID(SqlConnection cnPortal, string loginid, bool bCheckInternalOnly)
        {
            string sMemberGUID = "";

            string sSQL = "select MemberGUID, MemberType from PORTAL_Members WITH (NOLOCK) Where AccountID = @loginid;";
            using (SqlCommand cmd = new SqlCommand(sSQL, cnPortal))
            {
                cmd.Parameters.Add(new SqlParameter("@loginid", loginid.Trim().ToLower()));
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.Read())
                {
                    if (bCheckInternalOnly)
                    {
                        if (dr["MemberType"].ToString() == "2")
                            sMemberGUID = dr["MemberGUID"].ToString();
                    }
                    else
                        sMemberGUID = dr["MemberGUID"].ToString();
                }
                dr.Close();
                dr = null;
            }

            return sMemberGUID;
        }
    }
    #endregion
}

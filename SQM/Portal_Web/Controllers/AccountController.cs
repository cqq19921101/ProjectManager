using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.Abstract;
using Lib_Portal_Domain.PortalLogicsForGeneralControllerAction;
using Lib_Portal_Domain.Concrete;
using Lib_Portal_Domain.Model;
using Portal_Web.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using System.Configuration;
using Lib_VMI.SharedLibs;
using System.Web.Configuration;
using Lib_Portal_MUIResources;
using Lib_VMI_Domain.SharedLibs;
using System.Text;
using System.Data.SqlClient;
using Lib_Portal_Domain_MUIResources;
using System.Threading;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;
using System.Web.Security;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture]
    public class AccountController : Controller
    {
        readonly ILoginAuth _LoginAuthProvider;
        protected PortalController_Account pc_Account = new PortalController_Account();

        public AccountController()
        {
            this._LoginAuthProvider = new LoginAuth_LiteOnADandPortalMember();
        }

        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Message = TempData["shortMessage"] == null ? "" : TempData["shortMessage"].ToString();
            LoginModel lm = new LoginModel();
            return this.pc_Account.Login_HttpGet(lm, _LoginAuthProvider, Session, HttpContext, Request, Response, Url) ? PartialView(lm) : RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel lm, string returnUrl)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lm.UserSelectedLocale);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            bool passwordIsNull = false;

            if (string.IsNullOrEmpty(lm.UserID))
            {
                lm.ErrMsg = Portal_Domain_MUI.PORTAL_MSG_POR011;
                return PartialView(lm);
            }
            else if (string.IsNullOrEmpty(lm.Password))
            {
                lm.ErrMsg = Portal_Domain_MUI.PORTAL_MSG_POR012;
                return PartialView(lm);
            }
            else
            {
                SqlConnection cn = DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB);
                passwordIsNull = VMI_Common_Helper.CheckExternalAccountNoPassword(lm.UserID, cn);
                DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            }

            if (!passwordIsNull)
            {
                if (this.pc_Account.Login_HttpPost(lm, _LoginAuthProvider, Session, HttpContext, Request, Response, Url))
                {
                    if (VMI_Common_Helper.AuthNonVMIUser(lm.UserID, lm.Password, true, Session))
                    {
                        VMI_Common_Helper.AddSOSAInMenu(lm.UserID, Session, Request, lm.UserSelectedLocale);
                        return RedirectToLocal(Request.ApplicationPath.ToString());
                    }
                    else
                    {
                        return PartialView(lm);
                    }
                }
                else
                {
                    // SOSA Url
                    VMI_Common_Helper.AddSOSAInMenu(lm.UserID, Session, Request, lm.UserSelectedLocale);
                    return RedirectToLocal(returnUrl);
                }
            }
            else
            {
                // external account first login
                return RedirectToAction("ResetPassword", new { Account = lm.UserID });
            }
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            ViewData["HideForm"] = "";
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ResetPassword(string Account, string txtPrimaryEmail)
        {
            ViewData["HideForm"] = "";
            StringBuilder sb = new StringBuilder();
            bool isCorrectEmail = false;
            ViewBag.txtPrimaryEmail = txtPrimaryEmail == null ? "" : txtPrimaryEmail.Trim();

            if (!string.IsNullOrEmpty(Account) && !string.IsNullOrEmpty(txtPrimaryEmail))
            {
                sb.Append(@"
Select Case When Count('x') = 0 Then Cast(0 As Bit) Else Cast(1 As Bit) End isCorrectEmail
From PORTAL_Members With (Nolock)
Where AccountID = @AccountID And PrimaryEmail = @PrimaryEmail; ");

                SqlConnection cn = DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB);

                using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
                {
                    if (cn.State == System.Data.ConnectionState.Closed)
                    {
                        cn.Open();
                    }

                    cmd.Parameters.AddWithValue("AccountID", Account);
                    cmd.Parameters.AddWithValue("PrimaryEmail", txtPrimaryEmail);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            isCorrectEmail = Convert.ToBoolean(dr["isCorrectEmail"]);
                        }
                    }
                }

                DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            }

            if (!isCorrectEmail)
            {
                ViewBag.Message = "Wrong email, please contact Buyer.";
            }
            else
            {
                string sMsg = SystemMgmt_Member_Helper.PerformForgotPassword(Account, txtPrimaryEmail);
                if (sMsg == "")
                {
                    ViewBag.Message = "The password of your accccount had been reset. please check your mailbox for next step.";
                    ViewData["HideForm"] = "HideForm";
                }
                else
                {
                    ViewBag.Message = sMsg;
                }
            }

            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect("/");
        }
        #endregion

        #region QRCode       
        [AllowAnonymous]
        [HttpPost]
        public string GetToken()
        {
            return QRCodeHelper.getToken();
        }

        [AllowAnonymous]
        [HttpPost]
        public string InitUUID(string token)
        {
            return JsonConvert.SerializeObject(QRCodeHelper.initUUID(token));
        }

        [AllowAnonymous]
        [HttpPost]
        public string PushUUID(string token, string UUID)
        {
            return JsonConvert.SerializeObject(QRCodeHelper.pushUUID(token, UUID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpire_Page(Order = 1)]
        public ActionResult RunAs(string RunAsMemberGUID)
        {
            SqlConnection cn = DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB);
            #region Get MemberGUID
            StringBuilder sb = new StringBuilder();
            string GUID = string.Empty;
            sb.Append(@"Select MemberGUID from PORTAL_Members where AccountID = @AccountID");

            using (SqlCommand cmd = new SqlCommand(sb.ToString(), cn))
            {
                cmd.Parameters.AddWithValue("AccountID", "liteon\\" + RunAsMemberGUID);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        GUID = dr["MemberGUID"].ToString();
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(GUID))
            {
                PortalUserProfile u = PortalUserProfileHelper.GetPortalUserProfile(cn, GUID);
                if (u == null)
                {
                    TempData["shortMessage"] = "You have no permission.";
                    return RedirectToAction("Login");
                }
                else
                {
                    SessionHelper.SetSessionValue(Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SessionLoginUserKey), new PortalUser(u));

                    PortalController_SystemMgmt pc_SystemMgmt = new PortalController_SystemMgmt();

                    bool bViaSSL = true;
                    if (HttpContext.Request.ServerVariables["HTTPS"].ToString() == "off")
                        bViaSSL = false;

                    pc_SystemMgmt.RunAs_HttpPost(Session, GUID, bViaSSL, Url);

                    #region Generate menu after Run As...
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
                        sLocaleString, GUID, "Portal-Main-Menu", bViaSSL, new UrlHelper(Request.RequestContext)));
                    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

                    #region SetUserIsAuthenticated
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                        1,
                        "liteon\\" + RunAsMemberGUID,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        "",
                        FormsAuthentication.FormsCookiePath);

                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    cookie.HttpOnly = true;

                    Response.Cookies.Add(cookie);
                    #endregion
                    // SOSA Url
                    VMI_Common_Helper.AddSOSAInMenu(u.AccountID, Session, Request, sLocaleString);

                    return RedirectToAction("Index", "Home");
                    #endregion
                }
            }
            else
            {
                TempData["shortMessage"] = "You have no permission.";
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            string r = this.pc_Account.Logout(Session, HttpContext, Response);

            if (r == "")
            {
                return RedirectToAction("Login", new { ReturnUrl = Request.ApplicationPath.ToString() });
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Err = r;
                return View();
            }
        }
        #endregion

        #region Membership Activate
        [AllowAnonymous]
        public ActionResult Activate(string r)
        {
            if (SystemMgmt_Member_Helper.ActivateMemberCheck(r))
            {
                ViewBag.ResetGUID = r;
                return PartialView("ActivateSetNewPassword");
            }
            else
                return PartialView("ActivateFail");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Activate(string ResetGUID, string pwdPWD1, string pwdPWD2)
        {
            string r = SecurityHelper.CheckActivationData(pwdPWD1.Trim(), pwdPWD2.Trim());
            if (r == "")
            {
                if (SystemMgmt_Member_Helper.PerformAccountActivate(ResetGUID, pwdPWD1.Trim()))
                    return PartialView("ActivateSuccess");
                else
                    return PartialView("ActivateFail");
            }
            else
            {
                ViewBag.ErrMsg = r;
                return PartialView("ActivateSetNewPassword");
            }
        }
        #endregion

        #region Change Password
        [ControllerActionAccessControl(Order = 1)]
        [UpdateSessionLastAccessTime(Order = 2)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ChangePassword()
        {
            if (PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberType == 1)
            {
                ViewBag.ErrMsg = "";
                return View();
            }
            else
                return View("ChangePassword_NotRequired");
        }

        [HttpPost]
        [ControllerActionAccessControl(Order = 1)]
        [UpdateSessionLastAccessTime(Order = 2)]
        public ActionResult ChangePassword(string pwdPWD0, string pwdPWD1, string pwdPWD2)
        {
            PortalUserProfile RunAsUser = PortalUserProfileHelper.GetRunAsUserProfile(HttpContext);
            if (RunAsUser.MemberType == 1)
            {
                ViewBag.SuccessMsg = "";
                ViewBag.ErrMsg = "";

                string r = SecurityHelper.CheckActivationData(pwdPWD1.Trim(), pwdPWD2.Trim());
                if (r == "")
                {
                    r = SystemMgmt_Member_Helper.ChangePassword(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                        RunAsUser.MemberGUID, pwdPWD1);
                    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
                    if (r == "")
                        ViewBag.SuccessMsg = Portal_MUI.ACCOUNT_CHANGEPASSWORD_MSG002;
                    else
                        ViewBag.ErrMsg = r;
                    return View();
                }
                else
                    ViewBag.ErrMsg = r;
                return View();
            }
            else
                return View("ChangePassword_NotRequired");
        }
        #endregion

        #region Forget Password
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string txtAccountID, string txtPrimaryEmail, string hidFlag)
        {
            ViewBag.hidFlag = "1";
            ViewBag.txtAccountID = txtAccountID == null ? "" : txtAccountID.Trim();
            ViewBag.txtPrimaryEmail = txtPrimaryEmail == null ? "" : txtPrimaryEmail.Trim();

            if (hidFlag != null)
            {
                string sErrMsg = SystemMgmt_Member_Helper.PerformForgotPassword(txtAccountID.Trim(), txtPrimaryEmail.Trim());
                if (sErrMsg == "")
                {
                    ViewBag.SuccessMsg = "The password of your accccount had been reset. please check your mailbox for next step.";
                    ViewBag.ErrMsg = "";
                }
                else
                {
                    ViewBag.SuccessMsg = "";
                    ViewBag.ErrMsg = sErrMsg;
                }
            }

            return PartialView();
        }
        #endregion

        #region BrowserIsNotSupport
        public ActionResult BrowserIsNotSupport()
        {
            return PartialView();
        }
        #endregion

        #region
        [ControllerActionAccessControl(Order = 1)]
        [UpdateSessionLastAccessTime(Order = 2)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult UpdateEmail()
        {
            PortalUserProfile profile = PortalUserProfileHelper.GetRunAsUserProfile(HttpContext);
            if (profile.MemberType == 1)
            {
                ViewBag.ErrMsg = "";
                ViewBag.PrimaryEmail = SystemMgmt_Member_Helper.GetPrimaryEmail(profile.AccountID);
                return View();
            }
            else
                return View("UpdateEmail_NotRequired");
        }

        [HttpPost]
        [ControllerActionAccessControl(Order = 1)]
        [UpdateSessionLastAccessTime(Order = 2)]
        public ActionResult UpdateEmail(string email, string password)
        {
            PortalUserProfile RunAsUser = PortalUserProfileHelper.GetRunAsUserProfile(HttpContext);
            if (RunAsUser.MemberType == 1)
            {
                ViewBag.SuccessMsg = "";
                ViewBag.ErrMsg = "";

                string r = string.Empty;
                r = SystemMgmt_Member_Helper.PerformUpdateEmail(
                    RunAsUser.AccountID,
                    password,
                    email,
                    RunAsUser);

                if (r == "Email change successfully.")
                {
                    if (VMIRoleHelper.IsRoleMember(RunAsUser.Roles, EBRole.GMSDefaultSupplier) || VMIRoleHelper.IsRoleMember(RunAsUser.Roles, EBRole.GMSSupplierAdmins))
                    {
                        bool isSuccess = false;
                        try
                        {
                            isSuccess = CallGMS(RunAsUser.AccountID, email);
                            if (!isSuccess)
                            {
                                r = "Sync email to GMS failure, please contact system administrator manager.";
                            }
                        }
                        catch
                        {
                            r = "Cannot sync email to GMS, please contact system administrator manager.";
                        }
                    }

                    ViewBag.SuccessMsg = Portal_MUI.ACCOUNT_UPDATEEMAIL_MSG0021;
                }
                else
                    ViewBag.ErrMsg = r;

                ViewBag.PrimaryEmail = SystemMgmt_Member_Helper.GetPrimaryEmail(RunAsUser.AccountID);
                return View();
            }
            else
                return View("UpdateEmail_NotRequired");
        }

        private static bool CallGMS(string sAccount, string sMail)
        {
            GMSService serv = new GMSService();
            GMSSrvResult rlt = serv.updateAccountEmail(sAccount, sMail);

            return (rlt != null && rlt.Result == "0");
        }
        #endregion
    }
}
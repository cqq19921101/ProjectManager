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
    //[UpdateSessionLastAccessTime(Order = 50)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class HomeController : Controller
    {
        #region From Visual Studio 2015 MVC 5 template
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
        #endregion
        private const double _SessionExpiredMinutes = 30; //Set Session Expire Time

        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 1)]
        public ActionResult Index()
        {
            return View(PortalUserProfileHelper.GetPortalUser(HttpContext));
        }

        public ActionResult Unauthorized()
        {
            return PartialView();
        }

        public ActionResult SessionExpired()
        {
            return PartialView();
        }

        public ActionResult BrowserIsNotSupport()
        {
            return PartialView();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckSessionExpired()
        {
            string r = string.Empty;
            try
            {
                double dComp = SessionExpireControlHelper.CompareSessionTime(Session);

                if (dComp > _SessionExpiredMinutes)
                {
                    SessionExpireControlHelper.StopPortalSessionStopWatch(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                        Session,
                        PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
                    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

                    r = "X";
                }
                else
                {
                    //reset DataTrackDatetime
                    SessionExpireControlHelper.ReSetDataLockTrackDateTime(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                        PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
                    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

                    r = string.Empty;
                }
            }
            catch
            {
                return "X";
            }

            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckIsVMIAdminRole()
        {
            string r = string.Empty;

            r = VMI_Authority_Helper.IsVMIAdminRoleWithCorpVendorCodeJSon(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

    }
}
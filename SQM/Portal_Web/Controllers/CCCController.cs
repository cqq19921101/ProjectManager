using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib_VMI_Domain.Model;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class CCCController : Controller
    {
        // GET: CCC
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCCCLink()
        {
            string r = CCC_Helper.getCCCLink(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
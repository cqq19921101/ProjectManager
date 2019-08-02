using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_VMI_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib_Portal_Domain;
using Lib_VMI_Domain.SharedLibs;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class GMSController : Controller
    {
        // GET: GMS
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult Index()
        {
            ViewBag.Message = Request.Params["Message"].ToString();
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetGMSLink()
        {
            return GMS_Helper.GetGMSLinkUrl(PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
        }
    }
}
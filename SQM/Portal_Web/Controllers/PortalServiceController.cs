using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Portal_Web.SupportClasses;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [Authorize(Order=30)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class PortalServiceController : Controller
    {
        //
        // GET: /PortalService/

        [HttpPost]
        public string AcquireLock(string DataKey)
        {
            if (SessionHelper.GetSessionObjectValue(HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SessionLoginUserKey)) == null)
                return "timeout";
            else
                return DataLockHelper.AcquireLock(DataKey,
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        }

        [HttpPost]
        public string ReleaseLock(string DataKey)
        {
            if (SessionHelper.GetSessionObjectValue(HttpContext.Session, PortalGlobalConstantHelper.GetConstant(enumPortalGlobalConstant.SessionKey_SessionLoginUserKey)) == null)
                return "timeout";
            else
                return DataLockHelper.ReleaseLock(DataKey,
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        }
    }
}

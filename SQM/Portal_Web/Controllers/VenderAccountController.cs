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
    public class VenderAccountController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: VenderAccount
        public ActionResult VenderAccount()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadJSonWithFilter(string SearchText1,string SearchText2)
        {
            string r = VenderAccount_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText1,SearchText2, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
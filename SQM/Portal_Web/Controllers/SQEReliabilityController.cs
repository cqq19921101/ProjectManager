using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using Portal_Web.SupportClasses;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{

    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]

    public class SQEReliabilityController : Controller
    {
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQEReliability
        public ActionResult SQEReliability()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadReliability(string SearchText, string MemberGUID)
        {
            if (MemberGUID.Equals(""))
            {
                MemberGUID= PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQMReliability_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText),
                  StringHelper.EmptyOrUnescapedStringViaUrlDecode(MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadReliInfo(string SearchText, string PlantText)
        {
            string r = SQMReliInfo_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                 ,StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantText)
                 ,PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPlantList()
        {
            string r = SQMReliInfo_Helper
                .GetPlantListData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                ,PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateReliInfo(SQMReliability DataItem)
        {
            //DataItem.UserName = PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).UserEName;
            string r = SQMReliability_Helper.UpdateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

    }
}
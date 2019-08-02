using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQECriticismController : Controller
    {
        // GET: SQECriticism
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQECriticism()
        {
            return View();
        }
        [HttpPost]
        public string LoadReliInfo(string SearchText,string PlantText)
        {
            string r = SQMReliInfo_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                ,StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                ,StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantText)
                ,PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismJSonWithFilter(string CriticismID, string MemberType)
        {
            string r = SQECriticism_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(CriticismID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCriticism(SQECriticism DataItem)
        {
            string r = SQECriticism_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCriticism(SQECriticism DataItem)
        {
            string r = SQECriticism_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCriticism(SQECriticism DataItem)
        {
            string r = SQECriticism_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismMapJSonWithFilter(string SearchText, string MemberGUID)
        {
            if (MemberGUID.Equals(""))
            {
                MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQECriticismMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                ,MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismInfoJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQE_ManufacturersBasicInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCriticismMap(SQECriticismMap DataItem,string MemberGUID)
        {
            string r = SQECriticismMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                ,MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCriticismMap(SQECriticismMap DataItem)
        {
            string r = SQECriticismMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCriticismMap(SQECriticismMap DataItem)
        {
            string r = SQECriticismMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticismType(string CriticismCategory, string CriticismUnit, string CriticismItem)
        {
            string r = SQECriticism_Helper.GetCriticismType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), CriticismCategory, CriticismUnit, CriticismItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExcelCriticism(string SearchText)
        {

            string errmeg = string.Empty;
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\uat651\SQM\" + fileNameGUID + ".xlsx";

            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            errmeg = SQECriticism_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText,
                localPath, PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return errmeg;

        }
    }
}
using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Modal;
using Lib_SQM_Domain.Model;
using Lib_VMI_Domain.Model;
using Portal_Web.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class SQMMRBController : Controller
    {
        const string sLocalPathBase = "UploadFile/";
        // GET: SQMMRB
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQMMRB()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMMRBJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQMMRB_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
   
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateSQMMRB(SQMMRBData DataItem)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMMRB_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, localPath, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetSQMMRBData(string SID)
        {
            string r = SQMMRB_Helper.GetMRBData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditSQMMRB(SQMMRBData DataItem)
        {
            string r = SQMMRB_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadFile(FileAttachmentInfo FA)
        {
            string r = string.Empty;

            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            r = SQMMRB_Helper.UploadFile(
                   DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                   PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                   FA,
                   sLocalPath,
                   sLocalUploadPath,
                   Server,
                   Request.ApplicationPath
                   );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMRBFilesDetail(String FGUID)
        {
            string r = SQMMRB_Helper.GetMRBFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                FGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
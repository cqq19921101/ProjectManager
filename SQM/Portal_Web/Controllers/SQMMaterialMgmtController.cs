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
    public class SQMMaterialMgmtController : Controller
    {
        const string sLocalPathBase = "UploadFile/";
        // GET: SQMMaterialMgmt
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQMSCAR()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }

        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQESCAR()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMSCARJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQMScarMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQESCARJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQMScarMgmt_Helper.GetSQEDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetSQMSCARData(string SID)
        {
            string r = SQMScarMgmt_Helper.GetSQMSCARData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Appove(string SID,string Type)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMScarMgmt_Helper.Appove(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SID), StringHelper.EmptyOrUnescapedStringViaUrlDecode(Type), localPath, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetScarD8(string SID, string LitNo, string DateCode)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMScarMgmt_Helper.GetScarD8(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SID), StringHelper.EmptyOrUnescapedStringViaUrlDecode(LitNo), StringHelper.EmptyOrUnescapedStringViaUrlDecode(DateCode),urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateSQMSCAR(SQMScarMgmt DataItem)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMScarMgmt_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, localPath, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QuerySBUVendorMail(string ERP_VND)
        {
            string r = SQMMailRMgmt_Helper.Query_SBUVendorCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ERP_VND.Trim());
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditSQMSCAR(SQMScarMgmt DataItem)
        {
            string r = SQMScarMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteSQMSCAR(SQMScarMgmt DataItem)
        {
            string r = SQMScarMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
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

            r = SQMScarMgmt_Helper.UploadFile(
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
        public string GetScarFilesDetail(String FGUID)
        {
            string r = SQMScarMgmt_Helper.GetSCarFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                FGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateSQMSCARData(SQMScarDataMgmt DataItem)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMScarMgmt_Helper.UpdateSQMSCARData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,urlPre
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateD8(SQMScarDataMgmt DataItem)
        {
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            string r = SQMScarMgmt_Helper.UpdateD8(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, urlPre
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using System.Web.Script.Serialization;
using Lib_Portal_Domain.PortalLogicsForGeneralControllerAction;
using Portal_Web.SupportClasses;
using Lib_SQM_Domain.Modal;
using Lib_VMI_Domain.Model;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]

    public class SQMTradersController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQMTraders
        public ActionResult SQMTraders()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }
        public string GetMapVendorCode()
        {
            string r = SystemMgmt_Traders_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadTradersr(string SearchText, string MemberType)
        {
            string r = SystemMgmt_Traders_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateTraders(SystemMgmt_Traders DataItem)
        {
            string r = SystemMgmt_Traders_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditTraders(SystemMgmt_Traders DataItem)
        {
            string r = SystemMgmt_Traders_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteTraders(SystemMgmt_Traders DataItem)
        {
            string r = SystemMgmt_Traders_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #region KeyInfo
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadTraders(string BasicInfoGUID,FileAttachmentInfo FA,string PrincipalProducts)
        {
            string vendorCode = GetMapVendorCode();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SystemMgmt_Traders_Helper.UploadIntroFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    PrincipalProducts,
                    BasicInfoGUID
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }

        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public FileResult DownloadTraders(string DataKey)
        {
            FileInfoForOutput fi = SystemMgmt_Traders_Helper.DownloadSQMFileByStream(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTradersDetail(string BasicInfoGUID)
        {
            //string vendorCode = GetMapVendorCode();
            string r = SystemMgmt_Traders_Helper.GetCriticalFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                BasicInfoGUID

                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTradersDetailLoad(string BasicInfoGUID)
        {
            //string vendorCode = GetMapVendorCode();
            string r = SystemMgmt_Traders_Helper.GetTradersDetailLoad(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                BasicInfoGUID

                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion


        #region KeyInfo2
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadTraders2(string BasicInfoGUID, FileAttachmentInfo FA, string PrincipalProducts)
        {
            string vendorCode = GetMapVendorCode();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SystemMgmt_Traders_Helper.UploadIntroFile2(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    PrincipalProducts,
                    BasicInfoGUID
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }

        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public FileResult DownloadTraders2(string DataKey)
        {
            FileInfoForOutput fi = SystemMgmt_Traders_Helper.DownloadSQMFileByStream2(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTradersDetail2(string BasicInfoGUID)
        {
            string r = SystemMgmt_Traders_Helper.GetCriticalFilesDetail2(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                BasicInfoGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion


    }

}
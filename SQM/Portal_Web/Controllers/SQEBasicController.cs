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
using Lib_SQM_Domain.Model;
using System.Text.RegularExpressions;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQEBasicController : Controller
    {
        const string sLocalPathBase = "UploadFile/";
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetUserGUID()
        {
            string r = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            return r;
        }

        #region KeyInfo
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult KeyInfoIntro()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadIntroFile(FileAttachmentInfo FA, String Type, String ValidDate)
        {
            string vendorCode = GetUserGUID();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SQMBasic_CriticalFile_Helper.UploadIntroFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    Type,
                    ValidDate
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadKeyInfoTypeJSonWithFilter()
        {
            string vendorCode = GetUserGUID();
            string r = SQMBasic_CriticalFile_Helper.LoadKeyInfoTypeJSonWithFilter(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), vendorCode);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public FileResult DownloadSQMFile(string DataKey)
        {
            FileInfoForOutput fi = SQMBasic_CriticalFile_Helper.DownloadSQMFileByStream(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }



        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticalFilesDetail(string vendorCode)
        {

            string r = SQMBasic_CriticalFile_Helper.GetCriticalFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCriticalFiles(TB_SQM_CriticalFile DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQMBasic_CriticalFile_Helper.EditCriticalFile(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticalFile(string vendorCode)
        {

            string r = SQMBasic_CriticalFile_Helper.GetCriticalFile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQualityEvent(string vendorCode)
        {

            string r = SQMBasic_CriticalFile_Helper.GetQualityEvent(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateQualityEvent(TB_SQM_QualityEvent DataItem, FileAttachmentInfo FA)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            DataItem.QEGUID = Guid.NewGuid().ToString();
            string r = SQMBasic_CriticalFile_Helper.CreateDataItemQE(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataItem
            );
            //上傳檔案
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
            if (r == "")
            {
                r = UploadQualityEventFile(FA, DataItem.QEGUID);
            }

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditQualityEvent(TB_SQM_QualityEvent DataItem, FileAttachmentInfo FA)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQMBasic_CriticalFile_Helper.EditQualityEvent(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            //上傳檔案
            if (r == "" && FA.SPEC != "[]")
            {
                string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
                string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
                r = UploadQualityEventFile(FA, DataItem.QEGUID);
            }

            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteQualityEvent(TB_SQM_QualityEvent DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQMBasic_CriticalFile_Helper.DeleteQualityEvent(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadQualityEventFile(FileAttachmentInfo FA, String QEGUID)
        {
            string vendorCode = GetUserGUID();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SQMBasic_CriticalFile_Helper.UploadQualityEventFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    QEGUID
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        #endregion

        #region EHS
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetParam(TB_SQM_APPLICATION_PARAM DataItem)
        {
            string r = SQM_Basic_Helper.GetParam(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                DataItem
                );

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetWasteHandler(string vendorCode)
        {

            string r = SQMBasic_CriticalFile_Helper.GetWasteHandler(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateWasteHandler(TB_SQM_WasteHandler DataItem, FileAttachmentInfo FA, FileAttachmentInfo FABL)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            DataItem.WHGUID = Guid.NewGuid().ToString();
            string r = SQMBasic_CriticalFile_Helper.CreateDataItemWH(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataItem
            );
            //上傳檔案
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
            if (r == "")
            {
                r = UploadWasteHandlerFile(FA, DataItem.WHGUID, "1");
            }
            sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FABL.SUBFOLDER);
            if (r == "")
            {
                r = UploadWasteHandlerFile(FABL, DataItem.WHGUID, "2");
            }

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditWasteHandler(TB_SQM_WasteHandler DataItem, FileAttachmentInfo FA, FileAttachmentInfo FABL)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQMBasic_CriticalFile_Helper.EditWasteHandler(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            //上傳檔案
            if (r == "" && FA.SPEC != "[]")
            {
                string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
                string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
                r = UploadQualityEventFile(FA, DataItem.WHGUID);
            }
            if (r == "" && FABL.SPEC != "[]")
            {
                string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FABL.SUBFOLDER);
                string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
                r = UploadQualityEventFile(FABL, DataItem.WHGUID);
            }

            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteWasteHandler(TB_SQM_WasteHandler DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQMBasic_CriticalFile_Helper.DeleteWasteHandler(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadWasteHandlerFile(FileAttachmentInfo FA, String WHGUID, string type)
        {
            string vendorCode = GetUserGUID();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SQMBasic_CriticalFile_Helper.UploadWasteHandlerFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    WHGUID,
                    type
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        #endregion

        #region 供應商通訊錄
        // GET: SQEContact
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQEContact()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadReliInfo(string SearchText, string PlantText)
        {
            string r = SQMReliInfo_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                 , StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantText)
                 , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadContact(string SearchText, string MemberGUID)
        {
            if (MemberGUID.Equals(""))
            {
                MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQEContact_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateContact(SQEContact DataItem, String MemberGUID)
        {
            string r = SQEContact_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditContact(SQEContact DataItem)
        {
            string r = SQEContact_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteContact(SQEContact DataItem)
        {
            string r = SQEContact_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        #endregion

        #region 供應商評測狀況
        // GET: SQECriticism
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQECriticism()
        {
            return View();
        }
        //[HttpPost]
        //public string LoadReliInfo(string SearchText, string PlantText)
        //{
        //    string r = SQMReliInfo_Helper
        //        .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
        //        , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
        //        , StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantText)
        //        , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

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
                , MemberGUID);
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
        public string CreateCriticismMap(SQECriticismMap DataItem, string MemberGUID)
        {
            string r = SQECriticismMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                , MemberGUID);
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
        #endregion

        #region PLV
        // GET: SQMPVL
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQMPVL()
        {

            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPVL(string VTNAME, string CNAME, string CSNAME)
        {
            string vendorCode = GetUserGUID();
            string r = SQM_PVL_Helper.GetBasicInfoTypes(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                VTNAME,
                CNAME,
                CSNAME,
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPVL(SystemMgmt_PVL DataItem)
        {
            //string r = string.Empty;
            //string vendorCode = GetUserGUID();
            //DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            //string errmeg = string.Empty;
            //string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            //string fileNameGUID = Guid.NewGuid().ToString();
            //string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            //string filenameNet = @"\\uat651\SQM\" + fileNameGUID + ".xlsx";
            //string appName = Request.ApplicationPath.ToString();
            string r = SQM_PVL_Helper.EditPVL(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
               PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string PVLContact(SystemMgmt_PVL DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            string errmeg = string.Empty;
            //string filename = "E:\\" + Guid.NewGuid() + ".xlsx";
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\uat651\SQM\" + fileNameGUID + ".xlsx";
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            SQM_PVL_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                localPath, PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            //try
            //{
            //    string tmpFileName = filename;
            //    FileInfo tmpFI = new FileInfo(tmpFileName);
            //    Response.Clear();
            //    Response.ClearHeaders();
            //    Response.Buffer = false;

            //    Response.AppendHeader("Content-Disposition", "attachment;filename= " + HttpUtility.UrlEncode(Path.GetFileName(tmpFileName),
            //    System.Text.Encoding.UTF8));
            //    Response.AppendHeader("Content-Length", tmpFI.Length.ToString());
            //    Response.ContentType = "application/octet-stream";
            //    Response.WriteFile(tmpFileName);
            //    Response.Flush();
            //    Response.End();
            //}
            //catch (Exception ex)
            //{
            //    errmeg = ex.ToString();
            //}
            return errmeg;

        }
        #endregion

        #region Reliability
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
                MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQMReliability_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText),
                  StringHelper.EmptyOrUnescapedStringViaUrlDecode(MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string LoadReliInfo(string SearchText, string PlantText)
        //{
        //    string r = SQMReliInfo_Helper
        //        .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
        //         , StringHelper.EmptyOrUnescapedStringViaUrlDecode(PlantText)
        //         , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPlantList()
        {
            string r = SQMReliInfo_Helper
                .GetPlantListData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
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
        #endregion

        #region Insp
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMInsp
        public ActionResult SQMInsp()
        {
            return View();
        }

        #region InspCode
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadInsp(string SearchText)
        {
            string r = SQMInsp_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteInsp(SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region InspMap

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadInspMap(string SearchText, String SID)
        {
            string r = SQMInspMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , SID == null ? "" : SID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteInspMap(SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
              );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion 
        #endregion

        #region SQEQuality
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQEQuality
        public ActionResult SQEQuality()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQM_Quality_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityInspJSonWithFilter(string SInspCode, String SInspDesc, string ReportSID)
        {
            string r = SQM_QualityInsp_Helper.GetDataToJQGridJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspCode)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspDesc)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(ReportSID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityFileJSonWithFilter(string SearchText, string ReportSID)
        {
            string r = SQM_QualityFile_Helper.GetDataToJQGridJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(ReportSID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}

using System;
using System.Web.Mvc;

using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using Portal_Web.SupportClasses;
using Lib_SQM_Domain.Modal;
using Lib_VMI_Domain.Model;
using Lib_SQM_Domain.Model;
using Lib_SQE_Domain.Model;

namespace Portal_Web.Controllers
{
    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]
    public class SQMBasicController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        #region SQM
        #region SQMBasic
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult BasicInfo()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }
        #endregion

        #region SQMBasicInfo
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SendMail(string content)
        {
            string r = SQM_Mail_Helper.SendMail(content);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetVendorType()
        {
            string r = SQM_Basic_Helper.GetVendorType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext)
                );

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBasicInfoTypes()
        {
            string vendorCode = GetUserGUID();
            string r = SQM_Basic_Helper.GetBasicInfoTypes(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateBasicInfoType(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SystemMgmt_BasicInfoType_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditBasicInfoType(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SystemMgmt_BasicInfoType_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateApproveCase(TB_SQM_Manufacturers_BasicInfo DataItem)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SystemMgmt_BasicInfoType_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SuspendApproveCase(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQM_Approve_Case_Helper.SuspendCase(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteBasicInfoType(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string r = SystemMgmt_BasicInfoType_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBasicData(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQM_Basic_Helper.GetBasicData(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }



        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCommodityList()
        {
            string r = SQM_Basic_Helper.GetCommodityList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExcelContact(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            DataItem.BasicInfoGUID = StringHelper.EmptyOrUnescapedStringViaUrlDecode(DataItem.BasicInfoGUID);
            string errmeg = string.Empty;
            //string filename = "E:\\" + Guid.NewGuid() + ".xlsx";
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";

            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            errmeg = SQM_Basic_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
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
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCommoditySubList(String MainID)
        {
            string r = SQM_Basic_Helper.GetCommoditySubList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                MainID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapVendorCode()
        {
            string r = SQM_Basic_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetUserGUID()
        {
            string r = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditBasicInfo(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQM_Basic_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditBasicInfo2(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQM_Basic_Helper.EditBasicInfoGenral(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditBasicInfoAbility(TB_SQM_Manufacturers_BasicInfo DataItem)
        {
            string vendorCode = GetUserGUID();
            DataItem.VendorCode = vendorCode;
            string r = SQM_Basic_Helper.EditBasicInfoAbility(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditVendorType(String TypeID)
        {
            string vendorCode = GetUserGUID();
            string r = SQM_Basic_Helper.EditVendorType(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), vendorCode, TypeID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region KeyInfo
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult KeyInfoIntro()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            CommonHelper.setUrlPre(Request);
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
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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

        [AllowAnonymous]
        public ActionResult Approve(string TaskID)
        {
            TaskID = SQM_Approve_Case_Helper.desDecryptBase64(TaskID);
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticalFilesDetail(string vendorCode)
        {
            if (vendorCode == null | vendorCode == "")
            {
                vendorCode = GetUserGUID();
            }
            string r = SQMBasic_CriticalFile_Helper.GetCriticalFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
            if (vendorCode == null || vendorCode == "")
            {
                vendorCode = GetUserGUID();
            }
            string r = SQMBasic_CriticalFile_Helper.GetCriticalFile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQualityEvent(String SearchText, String vendorCode)
        {
            if (vendorCode == null || vendorCode == "")
            {
                vendorCode = GetUserGUID();

            }
            //string vendorCode = GetUserGUID();
            string r = SQMBasic_CriticalFile_Helper.GetQualityEvent(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                DataItem
                );

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetWasteHandler(String SearchText, String vendorCode)
        {
            if (vendorCode == null || vendorCode == "")
            {
                vendorCode = GetUserGUID();
            }
            string r = SQMBasic_CriticalFile_Helper.GetWasteHandler(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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

        #region Customers
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCustomersJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQM_CustomersMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCustomers(SQM_CustomersMgmt DataItem)
        {
            string r = SQM_CustomersMgmt_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCustomers(SQM_CustomersMgmt DataItem)
        {
            string r = SQM_CustomersMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCustomers(SQM_CustomersMgmt DataItem)
        {
            string r = SQM_CustomersMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string HPVendorNumUpdate(string HPVendorNum, string BasicInfoGUID)
        {
            string r = SQM_CustomersMgmt_Helper.HPVendorNumUpdate(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), BasicInfoGUID, HPVendorNum,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region  供應商通訊錄

        #region SQMContact
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQMContact()
        {
            return View();
        }
        #endregion
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadContactJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQMMgmt_Contact_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateContact(SQMMgmt_Contact DataItem)
        {
            string r = SQMMgmt_Contact_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditContact(SQMMgmt_Contact DataItem)
        {
            string r = SQMMgmt_Contact_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteContact(SQMMgmt_Contact DataItem)
        {
            string r = SQMMgmt_Contact_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetJobList()
        {
            string r = SQMMgmt_Contact_Helper.GetJobList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region 供應商評鑒狀況
        // GET: Criticism
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult Criticism()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismJSonWithFilter(string CriticismID, string MemberType)
        {
            string r = SQM_CriticismMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(CriticismID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCriticism(SQM_CriticismMgmt DataItem)
        {
            string r = SQM_CriticismMgmt_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCriticism(SQM_CriticismMgmt DataItem)
        {
            string r = SQM_CriticismMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCriticism(SQM_CriticismMgmt DataItem)
        {
            string r = SQM_CriticismMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismMapJSonWithFilter(string SearchText, string MemberGUID)
        {
            if (MemberGUID==null|| MemberGUID == "")
            {
                MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQM_CriticismMapMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCriticismInfoJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQM_ManufacturersBasicInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCriticismMap(SQM_CriticismMapMgmt DataItem, string MemberGUID)
        {
            string r = SQM_CriticismMapMgmt_Helper.CreateDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                , MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCriticismMap(SQM_CriticismMapMgmt DataItem)
        {
            string r = SQM_CriticismMapMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCriticismMap(SQM_CriticismMapMgmt DataItem)
        {
            string r = SQM_CriticismMapMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticismType(string CriticismCategory, string CriticismUnit, string CriticismItem)
        {
            string r = SQM_CriticismMgmt_Helper.GetCriticismType(
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
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";

            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            errmeg = SQM_CriticismMgmt_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText,
                localPath, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, urlPre);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return errmeg;

        }
        #endregion

        #region Agents
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQMAgents
        public ActionResult SQMAgents()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            CommonHelper.setUrlPre(Request);
            return View();
        }
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetVendorType()
        //{
        //    string r = SystemMgmt_Agents_Helper.GetVendorType(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
        //        );

        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetBasicData()
        //{
        //    string vendorCode = GetMapVendorCodeOfAgents();
        //    string r = SystemMgmt_Agents_Helper.GetBasicData(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCriticalFile()
        //{
        //    string vendorCode = GetMapVendorCodeOfAgents();
        //    string r = SystemMgmt_Agents_Helper.GetCriticalFile(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCommodityList()
        //{
        //    string r = SystemMgmt_Agents_Helper.GetCommodityList(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCommoditySubList(String MainID)
        //{
        //    string r = SystemMgmt_Agents_Helper.GetCommoditySubList(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        MainID
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetMapVendorCodeOfAgents()
        //{
        //    string r = SystemMgmt_Agents_Helper.GetMapVendorCode(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadAgents(string SearchText, string MemberType)
        {
            string r = SystemMgmt_Agents_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateAgents(SystemMgmt_Agents DataItem)
        {
            string r = SystemMgmt_Agents_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditAgents(SystemMgmt_Agents DataItem)
        {
            string r = SystemMgmt_Agents_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAgents(SystemMgmt_Agents DataItem)
        {
            string r = SystemMgmt_Agents_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #region KeyInfo
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadAgents(string BasicInfoGUID, FileAttachmentInfo FA, string PrincipalProducts)
        {
            string vendorCode = GetMapVendorCode();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SystemMgmt_Agents_Helper.UploadIntroFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
        public FileResult DownloadAgents(string DataKey)
        {
            FileInfoForOutput fi = SystemMgmt_Agents_Helper.DownloadSQMFileByStream(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAgentsDetail(string BasicInfoGUID)
        {
            string r = SystemMgmt_Agents_Helper.GetCriticalFilesDetail(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                BasicInfoGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAgentsDetailLoad(string BasicInfoGUID)
        {
            string r = SystemMgmt_Agents_Helper.GetAgentsDetailLoad(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                BasicInfoGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadAgents2(string SearchText, string MemberType)
        {
            string r = SystemMgmt_AgentsA2_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateAgents2(SystemMgmt_AgentsA2 DataItem)
        {
            string r = SystemMgmt_AgentsA2_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditAgents2(SystemMgmt_AgentsA2 DataItem)
        {
            string r = SystemMgmt_AgentsA2_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAgents2(SystemMgmt_AgentsA2 DataItem)
        {
            string r = SystemMgmt_AgentsA2_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion

        #region Equipment
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMEquipment
        public ActionResult SQMEquipment()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetEquipmentSpecialType(String MainID)
        {
            string r = SQM_EquipmentMgmt_Helper.GetEquipmentSpecialType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), MainID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetEquipmentType()
        {
            string r = SQM_EquipmentMgmt_Helper.GetEquipmentType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadEquipmentJSonWithFilter(string EquipmentType, string EquipmentSpecialType, string BasicInfoGUID, string MemberType)
        {
            string r = SQM_EquipmentMgmt_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentSpecialType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(BasicInfoGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string getModel(string EquipmentType, string EquipmentSpecialType, string MemberType)
        {
            string r = SQM_EquipmentMgmt_Helper.GetModel(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentType), StringHelper.EmptyOrUnescapedStringViaUrlDecode(EquipmentSpecialType));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteEquipment(SQM_EquipmentMgmt DataItem)
        {
            string r = SQM_EquipmentMgmt_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Product
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: /SQMProduct/
        public ActionResult SQMProduct()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadProduct(string SearchText, string MemberType, string BasicInfoGUID)
        {
            string r = SystemMgmt_Pro_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), StringHelper.EmptyOrUnescapedStringViaUrlDecode(BasicInfoGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreatePro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditPro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeletePro(SystemMgmt_Pro DataItem)
        {
            string r = SystemMgmt_Pro_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Traders
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQMTraders
        public ActionResult SQMTraders()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            CommonHelper.setUrlPre(Request);
            return View();
        }

        //public string GetMapVendorCodeOfTraders()
        //{
        //    string r = SystemMgmt_Traders_Helper.GetMapVendorCode(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

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
        public string UploadTraders(string BasicInfoGUID, FileAttachmentInfo FA, string PrincipalProducts)
        {
            string vendorCode = GetMapVendorCode();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SystemMgmt_Traders_Helper.UploadIntroFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
            //string vendorCode = GetMapVendorCodeOfTraders();
            string r = SystemMgmt_Traders_Helper.GetCriticalFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                BasicInfoGUID

                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetTradersDetailLoad(string BasicInfoGUID)
        {
            //string vendorCode = GetMapVendorCodeOfTraders();
            string r = SystemMgmt_Traders_Helper.GetTradersDetailLoad(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
                    PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
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
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext),
                BasicInfoGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #endregion 
        #endregion
        #region SQE
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetUserGUID()
        //{
        //    string r = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
        //    return r;
        //}

        #region KeyInfo
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQEKeyInfoIntro()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string UploadIntroFile(FileAttachmentInfo FA, String Type, String ValidDate)
        //{
        //    string vendorCode = GetUserGUID();
        //    string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //    string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

        //    string r = SQMBasic_CriticalFile_Helper.UploadIntroFile(
        //            DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //            PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //            FA,
        //            vendorCode,
        //            sLocalPath,
        //            sLocalUploadPath,
        //            Server,
        //            Request.ApplicationPath,
        //            Type,
        //            ValidDate
        //            );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

        //    return r;
        //}
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadKeyInfoTypeJSonWithFilter()
        {
            string vendorCode = GetUserGUID();
            string r = SQMBasic_CriticalFile_Helper.LoadKeyInfoTypeJSonWithFilter(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), vendorCode);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        //[HttpGet]
        //[SessionExpire_Service(Order = 1)]
        //public FileResult DownloadSQMFile(string DataKey)
        //{
        //    FileInfoForOutput fi = SQMBasic_CriticalFile_Helper.DownloadSQMFileByStream(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        DataKey);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        //}



        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCriticalFilesDetail(string vendorCode)
        //{

        //    string r = SQMBasic_CriticalFile_Helper.GetCriticalFilesDetail(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditCriticalFiles(TB_SQM_CriticalFile DataItem)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SQMBasic_CriticalFile_Helper.EditCriticalFile(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCriticalFile(string vendorCode)
        //{

        //    string r = SQMBasic_CriticalFile_Helper.GetCriticalFile(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetQualityEvent(string vendorCode)
        //{

        //    string r = SQMBasic_CriticalFile_Helper.GetQualityEvent(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateQualityEvent(TB_SQM_QualityEvent DataItem, FileAttachmentInfo FA)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    DataItem.QEGUID = Guid.NewGuid().ToString();
        //    string r = SQMBasic_CriticalFile_Helper.CreateDataItemQE(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        DataItem
        //    );
        //    //上傳檔案
        //    string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //    string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
        //    if (r == "")
        //    {
        //        r = UploadQualityEventFile(FA, DataItem.QEGUID);
        //    }

        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditQualityEvent(TB_SQM_QualityEvent DataItem, FileAttachmentInfo FA)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SQMBasic_CriticalFile_Helper.EditQualityEvent(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

        //    //上傳檔案
        //    if (r == "" && FA.SPEC != "[]")
        //    {
        //        string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //        string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
        //        r = UploadQualityEventFile(FA, DataItem.QEGUID);
        //    }

        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string DeleteQualityEvent(TB_SQM_QualityEvent DataItem)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SQMBasic_CriticalFile_Helper.DeleteQualityEvent(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string UploadQualityEventFile(FileAttachmentInfo FA, String QEGUID)
        //{
        //    string vendorCode = GetUserGUID();
        //    string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //    string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

        //    string r = SQMBasic_CriticalFile_Helper.UploadQualityEventFile(
        //            DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //            PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //            FA,
        //            vendorCode,
        //            sLocalPath,
        //            sLocalUploadPath,
        //            Server,
        //            Request.ApplicationPath,
        //            QEGUID
        //            );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

        //    return r;
        //}
        //#endregion

        //#region EHS
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetParam(TB_SQM_APPLICATION_PARAM DataItem)
        //{
        //    string r = SQM_Basic_Helper.GetParam(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        DataItem
        //        );

        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetWasteHandler(string vendorCode)
        //{

        //    string r = SQMBasic_CriticalFile_Helper.GetWasteHandler(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //        vendorCode
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateWasteHandler(TB_SQM_WasteHandler DataItem, FileAttachmentInfo FA, FileAttachmentInfo FABL)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    DataItem.WHGUID = Guid.NewGuid().ToString();
        //    string r = SQMBasic_CriticalFile_Helper.CreateDataItemWH(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //        DataItem
        //    );
        //    //上傳檔案
        //    string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //    string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
        //    if (r == "")
        //    {
        //        r = UploadWasteHandlerFile(FA, DataItem.WHGUID, "1");
        //    }
        //    sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FABL.SUBFOLDER);
        //    if (r == "")
        //    {
        //        r = UploadWasteHandlerFile(FABL, DataItem.WHGUID, "2");
        //    }

        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditWasteHandler(TB_SQM_WasteHandler DataItem, FileAttachmentInfo FA, FileAttachmentInfo FABL)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SQMBasic_CriticalFile_Helper.EditWasteHandler(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

        //    //上傳檔案
        //    if (r == "" && FA.SPEC != "[]")
        //    {
        //        string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //        string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
        //        r = UploadQualityEventFile(FA, DataItem.WHGUID);
        //    }
        //    if (r == "" && FABL.SPEC != "[]")
        //    {
        //        string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FABL.SUBFOLDER);
        //        string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");
        //        r = UploadQualityEventFile(FABL, DataItem.WHGUID);
        //    }

        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string DeleteWasteHandler(TB_SQM_WasteHandler DataItem)
        //{
        //    string vendorCode = GetUserGUID();
        //    DataItem.VendorCode = vendorCode;
        //    string r = SQMBasic_CriticalFile_Helper.DeleteWasteHandler(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string UploadWasteHandlerFile(FileAttachmentInfo FA, String WHGUID, string type)
        //{
        //    string vendorCode = GetUserGUID();
        //    string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
        //    string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

        //    string r = SQMBasic_CriticalFile_Helper.UploadWasteHandlerFile(
        //            DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
        //            PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
        //            FA,
        //            vendorCode,
        //            sLocalPath,
        //            sLocalUploadPath,
        //            Server,
        //            Request.ApplicationPath,
        //            WHGUID,
        //            type
        //            );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

        //    return r;
        //}
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
            if (MemberGUID == null || MemberGUID == "")
            {
                MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQEContact_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateContact(SQEContact DataItem, String MemberGUID)
        //{
        //    string r = SQEContact_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditContact(SQEContact DataItem)
        //{
        //    string r = SQEContact_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string DeleteContact(SQEContact DataItem)
        //{
        //    string r = SQEContact_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}


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

        //[SessionExpire_Service(Order = 1)]
        //public string LoadCriticismJSonWithFilter(string CriticismID, string MemberType)
        //{
        //    string r = SQECriticism_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(CriticismID));
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateCriticism(SQECriticism DataItem)
        //{
        //    string r = SQECriticism_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditCriticism(SQECriticism DataItem)
        //{
        //    string r = SQECriticism_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string DeleteCriticism(SQECriticism DataItem)
        //{
        //    string r = SQECriticism_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string LoadCriticismMapJSonWithFilter(string SearchText, string MemberGUID)
        //{
        //    if (MemberGUID.Equals(""))
        //    {
        //        MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
        //    }
        //    string r = SQECriticismMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
        //        , MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string LoadCriticismInfoJSonWithFilter(string SearchText, string MemberType)
        //{
        //    string r = SQE_ManufacturersBasicInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText));
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string CreateCriticismMap(SQECriticismMap DataItem, string MemberGUID)
        //{
        //    string r = SQECriticismMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
        //        , MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}

        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string EditCriticismMap(SQECriticismMap DataItem)
        //{
        //    string r = SQECriticismMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string DeleteCriticismMap(SQECriticismMap DataItem)
        //{
        //    string r = SQECriticismMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
        //        PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
        //        PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string GetCriticismType(string CriticismCategory, string CriticismUnit, string CriticismItem)
        //{
        //    string r = SQECriticism_Helper.GetCriticismType(
        //        DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), CriticismCategory, CriticismUnit, CriticismItem
        //        );
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return r;
        //}


        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        //public string ExcelCriticism(string SearchText)
        //{

        //    string errmeg = string.Empty;
        //    string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
        //    string fileNameGUID = Guid.NewGuid().ToString();
        //    string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
        //    string filenameNet = @"\\uat651\SQM\" + fileNameGUID + ".xlsx";

        //    string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
        //    errmeg = SQECriticism_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText,
        //        localPath, PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID, urlPre);
        //    DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
        //    return errmeg;

        //}
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
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            SQM_PVL_Helper.CreatExcel(filename, filenameNet, DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                localPath, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, urlPre);
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
            if (MemberGUID == null || MemberGUID == "")
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
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext));
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
        public ActionResult SQEInsp()
        {
            return View();
        }

        #region InspCode
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadInsp(string SearchText, SQMInsp DataItem)
        {
            string r = SQMInsp_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                ,DataItem
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
        public string LoadInspMap(string SearchText, SQMInspMap DataItem)
        {
            string r = SQMInspMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem);
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
        public string LoadQualityJSonWithFilter(string SearchText, SQM_Quality DataItem, string MemberType)
        {
            string r = SQM_Quality_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, DataItem, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityInspJSonWithFilter(string SInspCode, String SInspDesc, SQM_QualityInsp DataItem)
        {
            string r = SQM_QualityInsp_Helper.GetDataToJQGridJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspCode)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SInspDesc)
                , DataItem
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

        #region AQL
        #region AQLPlant
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQEAQLPlant
        public ActionResult SQEAQLPlant()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadAQLPlant(string SearchText, SQEAQLPlant DataItem)
        {
            string r = SQEAQLPlant_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateAQLPlant(SQEAQLPlant DataItem)
        {
            string r = SQEAQLPlant_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditAQLPlant(SQEAQLPlant DataItem)
        {
            string r = SQEAQLPlant_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAQLPlant(SQEAQLPlant DataItem)
        {
            string r = SQEAQLPlant_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region AQLPlantMap
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQEAQLPlantMap
        public ActionResult SQEAQLPlantMap()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadAQLPlantMap(string SearchText, SQEAQLPlantMap DataItem)
        {
            string r = SQEAQLPlantMap_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateAQLPlantMap(SQEAQLPlantMap DataItem)
        {
            string r = SQEAQLPlantMap_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditAQLPlantMap(SQEAQLPlantMap DataItem)
        {
            string r = SQEAQLPlantMap_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAQLPlantMap(SQEAQLPlantMap DataItem)
        {
            string r = SQEAQLPlantMap_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapAQLType(SQEAQLPlantMap DataItem)
        {
            string r = SQEAQLPlantMap_Helper
                .GetMapAQLType(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion
        #region AQLPlantRule
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQEAQLPlantRule
        public ActionResult SQEAQLPlantRule()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadAQLPlantRule(string SearchText, SQEAQLPlantRule DataItem)
        {
            string r = SQEAQLPlantRule_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateAQLPlantRule(SQEAQLPlantRule DataItem)
        {
            string r = SQEAQLPlantRule_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditAQLPlantRule(SQEAQLPlantRule DataItem)
        {
            string r = SQEAQLPlantRule_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAQLPlantRule(SQEAQLPlantRule DataItem)
        {
            string r = SQEAQLPlantRule_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DisabledAQLPlantRule(SQEAQLPlantRule DataItem)
        {
            string r = SQEAQLPlantRule_Helper.DisabledDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        public string GetPlantNameList()
        {
            string r = SQEAQLPlantRule_Helper
                .GetPlantNameList(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        public string GetPlantMapList(string MainID)
        {
            string r = SQEAQLPlantRule_Helper
                .GetPlantMapList(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext)
                ,MainID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        //ExcelAQLRule
        #endregion

        #region UD Maintain
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQEUDMaintain()
        {
            return View();
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadUDJSonWithFilter(string SearchText, string MemberType)
        {
            string r = SQE_UD_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                SearchText,
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateUD(SQE_UD DataItem)
        {
            string r = SQE_UD_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditUD(SQE_UD DataItem)
        {
            string r = SQE_UD_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteUD(SQE_UD DataItem)
        {
            string r = SQE_UD_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region LitNO_Plant
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQELitNO_Plant()
        {
            return View();
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadLitNO_PlantJSonWithFilter(string SearchText)
        {
            string r = SQE_LitNO_Plant_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                SearchText,
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateLitNO_Plant(SQE_LitNO_Plant DataItem)
        {
            string r = SQE_LitNO_Plant_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditLitNO_Plant(SQE_LitNO_Plant DataItem)
        {
            string r = SQE_LitNO_Plant_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteLitNO_Plant(SQE_LitNO_Plant DataItem)
        {
            string r = SQE_LitNO_Plant_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetLitNoUnit(string LitNo)
        {
            string r = SQE_LitNO_Plant_Helper.GetLitNoUnit(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), LitNo,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetLitNo()
        {
            string r = SQE_LitNO_Plant_Helper.GetLitNo(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), 
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
              );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion
        #endregion
    }
}

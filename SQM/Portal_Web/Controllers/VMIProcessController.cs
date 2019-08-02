using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Portal_Web.SupportClasses;
using Lib_VMI_Domain.Model;
using System.Diagnostics;
using Lib_Portal_Domain.Model;
using System.Web.Script.Serialization;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class VMIProcessController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        // GET: VMIProcess
        public ActionResult Index()
        {
            return View();
        }

        #region ToDoASN
        #region ASN_01_XX To Do ASN
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ToDoASN(string sQueryASNNoFrom)
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");

            if (sQueryASNNoFrom != null)
                ViewBag.QueryASNNoFrom = sQueryASNNoFrom;
            else
                ViewBag.QueryASNNoFrom = string.Empty;

            return View();
        }
        [HttpPost]
        public ActionResult RetrieveFileByFileKey(string FileKey, string FileName)
        {
            FileInfoForOutput fi = VMI_Process_Helper.RetrieveFileByFileKey(FileKey, FileName);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            if (!(fi == null))
                return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
            else
                return View("Unauthorized");
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckIsEnableToDoASNFunctionByType(string ASN_NUM, bool bIsAllCheck)
        {
            string r = VMI_Process_Helper.CheckIsEnableASNFunctionByType(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               ASN_NUM,
               bIsAllCheck);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string AuthAttachFileIsPass(string ASNNUM)
        {
            string r = VMI_Process_Helper.AuthAttachFile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string AuthEditShippingInfo(string ASNNUM)
        {
            string r = VMI_Process_Helper.AuthEditShippingInfo(
                 DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                 PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                 ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryHeaderInfoForEditShipInfo(string ASNNUM)
        {
            string r = VMI_Process_Helper.QueryASNHeaderInfoForUpdateOfQueryASN(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNInfoJsonWithFilter(VMI_Process_QueryItemForToDoASN QueryItem)
        {
            string r = VMI_Process_Helper.Query_ToDoASNList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckPassToCreateASNHeader(string PLANT, string ERP_VND)
        {
            string r = VMI_Process_Helper.CheckIsPassForCreateASNHeader(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                ERP_VND);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNHeaderInfoForCreate(string PLANT, string ERP_VND)
        {
            string r = VMI_Process_Helper.QueryASNHeaderInfoForCreate(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                ERP_VND);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateToDoASNHeaderInfoForCreate(VMI_Process_ToDoASNHeadInfo TDASNHI)
        {
            string r = VMI_Process_Helper.CreateASNHeaderInfoForCreate(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               TDASNHI);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNHeaderInfoForManage(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNHeaderInfoForManage(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               ASN_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNDetailInfoJsonWithFilter(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNDetailInfoForManage(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               ASN_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNHeaderInfoForUpdate(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNHeaderInfoForUpdate(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASN_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateToDoASNHeaderInfo(VMI_Process_ToDoASNHeadInfo TDASNHI)
        {
            string r = VMI_Process_Helper.UpdateASNHeaderInfo(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               TDASNHI);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateHeaderForEditShipInfo(VMI_Process_ToDoASNHeadInfo TDASNHI)
        {
            string r = VMI_Process_Helper.UpdateASNHeaderInfoForUpdateOfQueryASN(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               TDASNHI);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNDetailInfoForCreate(string ASN_NUM, string PO_NUM, string PO_LINE, string PLANT, string VENDOR, string ETA)
        {
            string r = VMI_Process_Helper.QueryASNDetailInfoForCreate(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASN_NUM,
                PO_NUM,
                PO_LINE,
                PLANT,
                VENDOR,
                ETA
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryContractCodeForASNDetail(string ASNNUM, string PLANT, string PO_NUM, string PO_LINE, string VENDOR, string MATERIAL, string ETA)
        {
            string r = VMI_Process_Helper.QueryContractCodeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUM,
                PLANT,
                PO_NUM,
                PO_LINE,
                VENDOR,
                MATERIAL,
                ETA
                );

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateToDoASNDetailInfoForCreate(VMI_Process_ToDoASNDetailInfo TDASNDI, string PLANT, string VENDOR, string ETA)
        {
            string r = VMI_Process_Helper.CreateASNDetailInfoForCreate(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                TDASNDI,
                PLANT,
                VENDOR,
                ETA
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNDetailInfoForModify(VMI_Process_ToDoASNDetailInfo TDASNDI, string ASNNUM, string ASNLINE, string PLANT, string VENDOR)
        {
            string r = VMI_Process_Helper.QueryASNDetailInfoForModify(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUM,
                ASNLINE,
                PLANT,
                VENDOR
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateToDoASNDetailInfo(VMI_Process_ToDoASNDetailInfo TDASNDI)
        {
            string r = VMI_Process_Helper.ModifyASNDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                TDASNDI
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteToDoASNDetailInfo(string ASN_NUM, string ASN_LINES)
        {
            string r = VMI_Process_Helper.DeleteASNDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM,
                ASN_LINES
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAllToDoASNDetailInfo(string ASN_NUM)
        {
            string r = VMI_Process_Helper.DeleteAllASNDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNImportDetailByBatchInfo(string PLANT, string VENDOR, string ETA, string MATERIALS, string QueryType, string page, int rows)
        {
            string r = VMI_Process_Helper.QueryASNImportDetailByBatchInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PLANT,
                VENDOR,
                ETA,
                MATERIALS,
                QueryType,
                page,
                rows
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateToDoASNImportDetailByBatchInfo(string ASNNUM, string ETA, string JsonImport)
        {
            string r = VMI_Process_Helper.CreateASNImportDetailByBatchInfo(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               ASNNUM,
               ETA,
               JsonImport
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNImportDetailByRealTimeInfo(string PLANT, string VENDOR, string ETA, string MATERIALS, string QueryType, string page, int rows)
        {
            string r = VMI_Process_Helper.QueryASNImportDetailRealTimeInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PLANT,
                VENDOR,
                ETA,
                MATERIALS,
                QueryType,
                page,
                rows
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNImportDetailShowASNQty(string PO_NUM, string PO_LINE)
        {
            string r = VMI_Process_Helper.QueryASNImportDetailShowASNQty(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PO_NUM,
                PO_LINE
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateToDoASNImportDetailByRTInfo(string ASNNUM, string JsonImport)
        {
            string r = VMI_Process_Helper.CreateASNImportDetailByRealTime(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASNNUM,
                JsonImport
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNDetailToExcel(string ASNNUM)
        {
            string fi = VMI_Process_Helper.DownloadASNDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                ASNNUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNFileDetailInfo(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNFileDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASN_NUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNFileViewLogsDetailInfo(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNFileViewLogsDetailInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASN_NUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadAttachmentForToDoASN(VMI_Process_ToDoASNAttachment TDAA)
        {
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, TDAA.SUBFOLDER);
            string r = VMI_Process_Helper.FileAttachASNInfo(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    TDAA,
                    sLocalPath
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateAttachmentRemarkForToDoASN(VMI_Process_ToDoASNAttachment TDAA)
        {
            string r = VMI_Process_Helper.UpdateToDoASNAttachRemarkInfo(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    TDAA
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteToDoASNAttachFileInfo(string ASN_NUM, string AttachGUIDs)
        {
            string r = VMI_Process_Helper.DeleteToDoASNAttachFileInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM,
                AttachGUIDs
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpGet]
        [SessionExpire_Service(Order = 1)]
        public FileResult DownloadToDoASNAttachFile(string DataKey)
        {
            FileInfoForOutput fi = VMI_Process_Helper.DownloadASNAttachFileByStream(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ReleaseToDoASN(string ASNNUM)
        {
            string r = VMI_Process_Helper.ReleaseToDoASN(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckProcessForReleaseStatus(string ASNNUM)
        {
            string r = VMI_Process_Helper.CheckReleaseStatus(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoASNDetailErrorInfo(VMI_Process_ToDoASNDetailInfo TDASNDI, string ASNNUM, string ASNLINE, string PLANT, string VENDOR)
        {
            string r = VMI_Process_Helper.QueryASNDetailErrorInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUM,
                ASNLINE,
                PLANT,
                VENDOR
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetForwarderInfo(string COMPANY_NAME, string ERP_VND)
        {
            string r = VMI_Process_Helper.GetForwarderInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                COMPANY_NAME,
                ERP_VND
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SubmitToBuyerReview(string ASN_NUM)
        {
            string r = VMI_Process_Helper.SubmitToBuyerReview(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RejectImportedASN(string ASN_NUM, string REJECT_REASON)
        {
            string r = VMI_Process_Helper.RejectImportedASN(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM,
                REJECT_REASON
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion
        #endregion

        #region ToDoVDS
        #region VDS_01_XX To Do VDS
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ToDoVDS()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetToDoVDSList(VMI_Process_QueryItemForToDoVDS QueryItem)
        {
            string r = VMI_Process_Helper.GetToDoVDSList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetToDoVDSTMType(VMI_Process_QueryItemForVDSInfo QueryItem)
        {
            string r = VMI_Process_Helper.GetVDSTMType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetToDoVDSHeader(VMI_Process_QueryItemForVDSHeader QueryItem)
        {
            string r = VMI_Process_Helper.GetVDSHeader(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetToDoVDSDetail(VMI_Process_QueryItemForVDSDetail QueryItem, string page)
        {
            string r = VMI_Process_Helper.GetVDSDetailJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem,
                page,
                false,
                VMI_Process_VDSFunc.ToDoVDS);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckVDSEditable(VMI_Process_QueryItemForVDSInfo QueryItem)
        {
            string r = VMI_Process_Helper.CheckVDSEditable(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                null,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateVDSDataMeasure(string updateInfo)
        {
            string r = VMI_Process_Helper.UpdateVDSDataMeasure(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                updateInfo
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryIfAllCommit(string VDS_NUM, string VRSIO)
        {
            string r = VMI_Process_Helper.QueryIfAllCommit(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                VDS_NUM,
                VRSIO
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateAllMaterialCommit(string VDS_NUM, string VRSIO, string COMMIT_CTL)
        {
            string r = VMI_Process_Helper.UpdateAllMaterialCommit(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                VDS_NUM,
                VRSIO,
                COMMIT_CTL
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UpdateMaterialCommit(string VDS_NUM, string VRSIO, string COMMIT_CTL, string Material)
        {
            string r = VMI_Process_Helper.UpdateMaterialCommit(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                VDS_NUM,
                VRSIO,
                COMMIT_CTL,
                Material
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryOpenPOCount(string PLANT, string VENDOR)
        {
            string r = VMI_Process_Helper.QueryOpenPOCount(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                VENDOR
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryOpenPODetail(string PLANT, string VENDOR, string page)
        {
            string r = VMI_Process_Helper.QueryOpenPODetailJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                VENDOR,
                page,
                false
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryInventoryCPUDT(string PLANT, string VENDOR)
        {
            string r = VMI_Process_Helper.QueryInventoryCPUDT(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                VENDOR
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryInventoryDetail(string PLANT, string VENDOR, string CPUDT, string page)
        {
            string r = VMI_Process_Helper.QueryInventoryDetailJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT,
                VENDOR,
                CPUDT,
                page,
                false
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CommitVDS(string VDS_INFO, bool isCommitAll)
        {
            string r = VMI_Process_Helper.CommitVDS(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                VDS_INFO,
                isCommitAll
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ToDoVDSExportVDS(VMI_Process_QueryItemForToDoVDS queryItem)
        {
            string fi = VMI_Process_Helper.ToDoVDSExportVDSInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                queryItem,
                VMI_Process_VDSFunc.ToDoVDS);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region VDS_03_XX Upload VDS
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult UploadVDS()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadVDSFile(FileAttachmentInfo FA, string Type)
        {
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);

            string r = VMI_Process_Helper.UploadVDSFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    Type,
                    sLocalPath,
                    Server,
                    Request.ApplicationPath
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        #endregion
        #endregion

        #region Upload ASN
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult UploadASN()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");

            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DownloadUploadASNTemplateFile()
        {
            string fi = VMI_Process_Helper.ExportUploadASNTemplateFile(Server, Request.ApplicationPath);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadASNFileInfo(VMI_Process_UploadFileInfo UFI, bool IsSplit, string SplitLine)
        {
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, UFI.SubFolder);
            string r = VMI_Process_Helper.UploadASNFile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                UFI,
                IsSplit,
                SplitLine,
                sLocalPath,
                Server,
                Request.ApplicationPath
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }

        #endregion

        #region ASN Special Cancel
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ASNSpecialCancel()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNSpecialCancelInfoJsonWithFilter(VMI_Process_QueryItemForToDoASN QueryItem)
        {

            string r = VMI_Process_Helper.Query_ASNSpecialCancelList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNSpecialCancelInfoForManage(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNSpecialHeaderInfoForManage(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               ASN_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNSpecialCancelDetailWithFilter(string ASN_NUM)
        {
            string r = VMI_Process_Helper.QueryASNDetailInfoForASNSpecialCancel(
              DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
              ASN_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ASNSpecialCancelByMultiASNNUM(string ASNNUMs)
        {
            string r = VMI_Process_Helper.CancelByMultiASNNUM(
             DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
             PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
             ASNNUMs);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ASNSpecialCancelByASNNUM(string ASNNUM)
        {
            string r = VMI_Process_Helper.CancelByASNNUM(
             DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
             PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
             ASNNUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ASNSpecialCancelByASNLINE(string ASNNUM, string ASNLINES)
        {
            string r = VMI_Process_Helper.CancelByASNNUMItem(
             DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
             PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
             ASNNUM,
             ASNLINES);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        #endregion

        #region Application Vendor Account
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ApplicationVendorAccount()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");

            PortalUserProfile RunAsUser = PortalUserProfileHelper.GetLoginUserProfile(HttpContext);
            ViewBag.Requestor = RunAsUser.UserEName;

            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAVASiteList()
        {
            string r = VMI_Process_Helper.GetAVASiteList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAVAList(AVAQueryItem queryItem)
        {
            string r = VMI_Process_Helper.GetAVAList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAVAInfo(AVAQueryItem queryItem)
        {
            List<AVA> AVAInfo = VMI_Process_Helper.GetAVAInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return new JavaScriptSerializer().Serialize(AVAInfo);
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteAVAItems(List<String> NVA_IDs)
        {
            string r = VMI_Process_Helper.DeleteAVAItems(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                NVA_IDs);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ProcessAVAItem(AVA AVAItem, FileAttachmentInfo FA)
        {
            string sLocalPath = (FA != null) ? FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER) : "";

            string r = VMI_Process_Helper.ProcessAVAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem,
                FA,
                sLocalPath);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExportAVAForm(AVA AVAItem)
        {
            string fi = VMI_Process_Helper.ExportAVAForm(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAccountList(AVA AVAItem)
        {
            string r = VMI_Process_Helper.GetAccountList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateVendorAccount(AVA AVAItem)
        {
            string r = VMI_Process_Helper.CreateVendorAccount(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RejectAVAItem(AVA AVAItem)
        {
            string r = VMI_Process_Helper.RejectAVAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string IsAddByVmiUser()
        {
            string r = VMI_Process_Helper.IsAddByVmiUser(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public bool isAVAAccess(AVA AVAItem)
        {
            bool r = VMI_Process_Helper.isAVAAccess(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DownloadAVAScanFile(AVA AVAItem, string FILE_NAME)
        {
            string fi = VMI_Process_Helper.DownloadAVAScanFile(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               AVAItem,
               FILE_NAME);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CloseAVAItem(AVA AVAItem)
        {
            string r = VMI_Process_Helper.CloseAVAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExistVendorAccount(AVA AVAItem)
        {
            string r = VMI_Process_Helper.ExistVendorAccount(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                AVAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region ToDoPOAck
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ToDoPOAck()
        {
            if (Request.QueryString["AutoQuery"] == "true")
            {
                ViewBag.AutoQuery = "true";
            }
            else
            {
                ViewBag.AutoQuery = "false";
            }
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryToDoPOAck(ToDoPOAckQueryItem queryItem)
        {
            string r = VMI_Process_Helper.QueryToDoPOAckJqGrid(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string AckPO(List<ToDoPOAck> POInfo)
        {
            string r = VMI_Process_Helper.AckPO(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                POInfo);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DownloadPOFile(ToDoPOAck POInfo)
        {
            string fi = VMI_Process_Helper.DownloadPOFile(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               POInfo);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        #endregion

        #region Vendor Email Query And Change
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult VendorEmailQueryAndChange()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetApplicationList(VendorEmailQueryItem queryItem)
        {
            string r = VMI_Process_Helper.GetApplicationList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string VendorEmailQueryEmail(VendorEmailQueryItem queryItem)
        {
            string r = VMI_Process_Helper.VendorEmailQueryEmail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RequestEmailChange(VendorEmailQueryItem queryItem)
        {
            string r = VMI_Process_Helper.RequestEmailChange(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ReviewVendorEmailApplyInfo(string ID)
        {
            string r = VMI_Process_Helper.ReviewVendorEmailApplyInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ChangeLockUser(string USER_GUID, bool isLocked)
        {
            string r = VMI_Process_Helper.ChangeLockUser(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                USER_GUID,
                isLocked);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ApproveVendorEmailChange(string ID, string USER_GUID, bool isLocked, string NEW_EMAIL)
        {
            string r = VMI_Process_Helper.ApproveVendorEmailChange(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ID,
                USER_GUID,
                isLocked,
                NEW_EMAIL);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RejectVendorEmailChange(string ID, string REJECT_REASON)
        {
            string r = VMI_Process_Helper.RejectVendorEmailChange(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ID,
                REJECT_REASON);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Unconfirmed Open PO Query & Export
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult OpenPOConfirmation()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryOpenPOConfirmationJqGrid(ConfrimOpenPOQueryItem queryItem)
        {
            string r = VMI_Process_Helper.QueryOpenPOConfirmationJqGrid(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string OpenPOConfirmationExportExcel(ConfrimOpenPOQueryItem QueryItem)
        {
            string fi = VMI_Process_Helper.OpenPOConfirmationExportExcel(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region Upload Open PO Confirmation
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult UploadOpenPOConfirmation()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadOpenPOConfirmationFile(FileAttachmentInfo FA)
        {
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);

            string r = VMI_Process_Helper.UploadOpenPOConfirmationFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    sLocalPath,
                    Server,
                    Request.ApplicationPath
                    );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }
        #endregion

        #region To Do Import List
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ToDoImportList()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryImportList(string IMPORT_LIST_NUM_FM, string IMPORT_LIST_NUM_TO, string IDN_NUM_FM, string IDN_NUM_TO, string IS_CLOSE)
        {
            string r = VMI_Process_Helper.QueryImportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM_FM,
                IMPORT_LIST_NUM_TO,
                IDN_NUM_FM,
                IDN_NUM_TO,
                IS_CLOSE);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryDNListForImportList(QueryItemForDNList queryItem)
        {
            string r = VMI_Process_Helper.QueryDNListForImportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditImportList(ImportList importList)
        {
            string r = VMI_Process_Helper.EditImportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                importList);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetImportListHeader(string IMPORT_LIST_NUM)
        {
            string r = VMI_Process_Helper.GetImportListHeader(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetImportListDetail(string IMPORT_LIST_NUM)
        {
            string r = VMI_Process_Helper.GetImportListDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetForwarderCompanyNameList(string IMPORT_LIST_NUM)
        {
            string r = VMI_Process_Helper.GetForwarderCompanyNameList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteImportList(string IMPORT_LIST_NUM)
        {
            string r = VMI_Process_Helper.DeleteImportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string PrintImportListExportPDF(string IMPORT_LIST_NUM)
        {
            string fi = VMI_Process_Helper.PrintImportListExportPDF(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string NoticeAndReleaseImportList(string IMPORT_LIST_NUM)
        {
            string r = VMI_Process_Helper.NoticeAndReleaseImportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                IMPORT_LIST_NUM);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAuthPlant()
        {
            string r = VMI_Process_Helper.GetAuthPlant(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPlantReceiveInfo(string PLANT)
        {
            string r = VMI_Process_Helper.GetPlantReceiveInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                PLANT);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Application Buyer Account
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ApplicationBuyerAccount()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");

            PortalUserProfile RunAsUser = PortalUserProfileHelper.GetLoginUserProfile(HttpContext);
            ViewBag.Requestor = RunAsUser.UserEName;

            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExportABAForm(ABA_ITEM ABAItem)
        {
            string fi = VMI_Process_Helper.ExportABAForm(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               ABAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SaveABAItem(ABA_ITEM ABAItem, FileAttachmentInfo FA)
        {
            string sLocalPath = (FA != null) ? FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER) : "";

            string r = VMI_Process_Helper.SaveABAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ABAItem,
                FA,
                sLocalPath);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetABAList(ABA_ITEM queryItem)
        {
            string r = VMI_Process_Helper.GetABAList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetABAInfo(ABA_ITEM queryItem)
        {
            List<ABA_ITEM> AVAInfo = VMI_Process_Helper.GetABAInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return new JavaScriptSerializer().Serialize(AVAInfo);
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DownloadABAScanFile(ABA_ITEM item)
        {
            string fi = VMI_Process_Helper.DownloadABAScanFile(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               item);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SubmitABAItem(ABA_ITEM ABAItem, FileAttachmentInfo FA)
        {
            string sLocalPath = (FA != null) ? FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER) : "";

            string r = VMI_Process_Helper.SubmitABAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ABAItem,
                FA,
                sLocalPath);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string RejectABAItem(ABA_ITEM ABAItem)
        {
            string r = VMI_Process_Helper.RejectABAItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ABAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateBuyerAccount(ABA_ITEM ABAItem)
        {
            string r = VMI_Process_Helper.CreateBuyerAccount(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ABAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadRoles()
        {
            string r = VMI_Process_Helper.LoadRoles(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteABAItems(string[] NBA_IDs)
        {
            string r = VMI_Process_Helper.DeleteABAItems(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                NBA_IDs);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public bool isABAAccess(ABA_ITEM ABAItem)
        {
            bool r = VMI_Process_Helper.isABAAccess(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ABAItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}
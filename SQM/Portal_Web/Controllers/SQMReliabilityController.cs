using System;
using System.Web.Mvc;
using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using Portal_Web.SupportClasses;
using Lib_SQM_Domain.Modal;
using Lib_VMI_Domain.Model;
using Lib_SQM_Domain.Model;
using System.Collections.Generic;

namespace Portal_Web.Controllers
{

    [GetUserProfileViaWindowsAuthentication(Order = 10)]
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [UpdateSessionLastAccessTime(Order = 50)]

    public class SQMReliabilityController : Controller
    {
        const string sLocalPathBase = "UploadFile/";
        #region Reliability
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQMReliability
        public ActionResult SQMReliability()
        {
            return View();
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadReliability(string SearchText, string MemberType)
        {
            string r = SQMReliability_Helper
                .GerDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText),
                  PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadReliabilityHistory(string SID)
        {
            if (SID.Equals(""))
            {
                SID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            }
            string r = SQMReliability_Helper
                .GetHistoryDateToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateReliability(SQMReliability DataItem)
        {
            string MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            string r = SQMReliability_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditReliability(SQMReliability DataItem)
        {
            string r = SQMReliability_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteReliability(SQMReliability DataItem)
        {
            string r = SQMReliability_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion

        #region  SQMQuality
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMQuality
        public ActionResult SQMQuality()
        {
            return View();
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadQualityJSonWithFilter(string SearchText, SQM_Quality DataItem,string MemberType)
        {
            string r = SQM_Quality_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, DataItem, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateQuality(SQM_Quality DataItem)
        {
            string r = SQM_Quality_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditQuality(SQM_Quality DataItem)
        {
            string r = SQM_Quality_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteQuality(SQM_Quality DataItem)
        {
            string r = SQM_Quality_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }


        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMQualityFile
        public ActionResult SQMQualityFile()
        {
            return View();
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

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateQualityFile(SQM_QualityFile DataItem)
        {
            string r = SQM_QualityFile_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditQualityFile(SQM_QualityFile DataItem)
        {
            string r = SQM_QualityFile_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteQualityFile(SQM_QualityFile DataItem)
        {
            string r = SQM_QualityFile_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadFileOfQuality(FileAttachmentInfo FA)
        {
            string r = string.Empty;

            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            r = SQM_QualityFile_Helper.UploadFile(
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


        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMQualityInsp
        public ActionResult SQMQualityInsp()
        {
            return View();
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
        public string CreateQualityInsp(SQM_QualityInsp DataItem)
        {
            string r = SQM_QualityInsp_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditQualityInsp(SQM_QualityInsp DataItem)
        {
            string r = SQM_QualityInsp_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem
              );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteQualityInsp(SQM_QualityInsp DataItem)
        {
            string r = SQM_QualityInsp_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetInspCodeList(String Insptype,string LitNo)
        {
            string r = SQM_QualityInsp_Helper.GetInspCodeList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                , Insptype, LitNo
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetInspDescList(String MainID)
        {
            string r = SQM_QualityInsp_Helper.GetInspDescList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                 StringHelper.EmptyOrUnescapedStringViaUrlDecode(MainID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetInspIfisOther(String SID, String SSID)
        {
            string r = SQM_QualityInsp_Helper.GetInspIfisOther(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                 StringHelper.EmptyOrUnescapedStringViaUrlDecode(SID)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SSID)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetReportTypeList()
        {
            string r = SQM_QualityInsp_Helper.GetReportTypeList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region  SQMCPK

        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMCPK
        public ActionResult SQMCPK()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }
        #region SQMCPK
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCPK(string SearchText)
        {
            string r = SQMCPK_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCPK(SQMCPK DataItem)
        {
            string r = SQMCPK_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCPK(SQMCPK DataItem)
        {
            DataItem.MemberGUID = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            string r = SQMCPK_Helper.EditDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCPK(SQMCPK DataItem)
        {
            string r = SQMCPK_Helper.DeleteDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                , PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetLitNoList()
        {
            string r = SQMCPK_Helper.GetLitNoList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetLitNoDataList(SQMCPK DataItem)
        {
            string r = SQMCPK_Helper.GetLitNoDataList(
                DataItem.plantCode,
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetLitNoData(String MainID,SQMCPK DataItem)
        {
            string r = SQMCPK_Helper.GetLitNoData(
                MainID,
                DataItem.plantCode,
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region SQMCPKSub
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCPKSub(string SearchText, SQMCPKSub DataItem)
        {
            string r = SQMCPKSub_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem
               );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCPKSub(SQMCPKSub DataItem)
        {
            string r = SQMCPKSub_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditCPKSub(SQMCPKSub DataItem)
        {
            string r = SQMCPKSub_Helper.EditDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteCPKSub(SQMCPKSub DataItem)
        {
            string r = SQMCPKSub_Helper.DeleteDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #region SQMCPKData
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadCPKData(string SearchText, SQMCPKData DataItem)
        {
            string r = SQMCPKData_Helper.GetDataToJQGridJson(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateCPKData(SQMCPKData DataItem)
        {
            string r = SQMCPKData_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            SendEmail(DataItem);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string SendEmail(SQMCPKData DataItem)
        {
            
             SQMCPKData_Helper.SendEmail(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem, PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return "";
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string StatisticCPKData(SQMCPKData DataItem)
        {
            string r = SQMCPKData_Helper.GetStatisticData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
        #endregion
        #region Upload
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadFile(string SID, FileAttachmentInfo FA)
        {
            string r = string.Empty;
            if (!SQMReliability_Helper.GetDataExistOrNull(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SID))
            {
                r = "請補充試驗項目信息";
                return r;
            }

            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            r = SQMReliability_Helper.UploadReliabilityFile(
                   DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                   PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                   FA,
                   sLocalPath,
                   sLocalUploadPath,
                   Server,
                   Request.ApplicationPath,
                   SID
                   );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }

        public string CommitFile(string SID, String FSID)
        {
            string errmeg = string.Empty;
            if (!SQMReliability_Helper.GetFileExistOrNull(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SID, FSID))
            {
                errmeg = "請先上傳文件";
                return errmeg;
            }

            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";
            string CaseID = string.Empty;
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            TB_SQM_Approve approve = new TB_SQM_Approve(FSID, "4");
            errmeg = TB_SQM_Approve_helper
               .CreateApproveCase(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , approve, filename, filenameNet, urlPre, ref CaseID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            errmeg += SQMReliability_Helper.UpdateFileCaseId(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , FSID
                , CaseID);
            return errmeg;
        }

        public string ExcelContact(SQMReliability DataItem)
        {
            string errmeg = string.Empty;


            TB_SQM_Approve approve = new TB_SQM_Approve(DataItem.ReliabilitySID, "3");

            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";
            string CaseID = string.Empty;
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);

            errmeg = TB_SQM_Approve_helper
                .CreateApproveCase(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , approve, filename, filenameNet, urlPre, ref CaseID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            errmeg += SQMReliability_Helper.UpdateCaseId(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
               , DataItem.ReliabilitySID
                , CaseID);
            return errmeg;

        }
        #endregion
        #region ECN
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        // GET: SQMQuality
        public ActionResult SQMECN()
        {
            CommonHelper.setUrlPre(Request);
            return View();
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadECNJSonWithFilter(string SearchText,  string MemberType)
        {
            string r = SQM_ECN_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), 
                SearchText,
                
                StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateECN(SQM_ECN DataItem)
        {
            string r = SQM_ECN_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditECN(SQM_ECN DataItem)
        {
            string r = SQM_ECN_Helper.EditDataItem(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                ,DataItem
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteECN(SQM_ECN DataItem)
        {
            string r = SQM_ECN_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetChangeTypeList()
        {
            string r = SQM_ECN_Helper.GetChangeTypeList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetChangeItemTypeList()
        {
            string r = SQM_ECN_Helper.GetChangeItemTypeList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ECNCommit(SQM_ECN DataItem)
        {
            string errmeg = string.Empty;
            TB_SQM_Approve approve;
            //如果製程，製造地點，人員,設備需要SQE簽核.其餘2項目簽核到RD
            List<string> changeItemTypeSID = new List<string>() { "1", "4", "5", "6" };
            if (changeItemTypeSID.Contains(DataItem.ChangeItemTypeSID))
            {
                approve = new TB_SQM_Approve(DataItem.MemberGUID, "5");
            }
            else
            {
                approve = new TB_SQM_Approve(DataItem.MemberGUID, "6");
            }

            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string fileNameGUID = Guid.NewGuid().ToString();
            string filename = localPath + @"UploadFile\SQM\" + fileNameGUID + ".xlsx";
            string filenameNet = @"\\"+ Request.Url.Authority.ToString() + @"\SQM\" + fileNameGUID + ".xlsx";
            string CaseID = string.Empty;
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);

            errmeg = TB_SQM_Approve_helper
                .CreateApproveCase(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
                , approve, filename, filenameNet, urlPre, ref CaseID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            errmeg += SQM_ECN_Helper.UpdateCaseId(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB)
               , DataItem
                , CaseID);
            return errmeg;

        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadFileECN(FileAttachmentInfo FA, String Type)
        {
            string r = string.Empty;

            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            r = SQM_ECN_Helper.UploadFile(
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
        public string GetECNFilesDetail(String FGUID)
        {
            string r = SQM_ECN_Helper.GetECNFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                FGUID
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region SQMBar
        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult SQMBar()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string getModel(string begin, string end, string Designator)
        {
            begin = begin.Replace("/", "");
            end = end.Replace("/","");
            string r = SQM_BAR_Helper.GetModel(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, StringHelper.EmptyOrUnescapedStringViaUrlDecode(Designator), StringHelper.EmptyOrUnescapedStringViaUrlDecode(begin), StringHelper.EmptyOrUnescapedStringViaUrlDecode(end));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadBARJSonWithFilter(string begin,string end, string Designator)
        { 
             begin = begin.Replace("/", "");
            end = end.Replace("/","");
            string r = SQM_BAR_Helper.getNewDate(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID, StringHelper.EmptyOrUnescapedStringViaUrlDecode(Designator),StringHelper.EmptyOrUnescapedStringViaUrlDecode(begin), StringHelper.EmptyOrUnescapedStringViaUrlDecode(end));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib_Portal_Domain.SupportClasses;
using Lib_Portal_Domain.SharedLibs;
using Lib_SQM_Domain.Model;
using Lib_VMI_Domain.Model;
using Lib_SQM_Domain.Modal;

namespace Portal_Web.Controllers
{
    public class SQMCertificationController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        // GET: SQMCertification
        public ActionResult CertificationInfo()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadSQMProduct1JSonWithFilter(string SearchText,string BasicInfoGUID, string MemberType)
        {
            string r = SQMCertification_CertificationInfo_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), StringHelper.EmptyOrUnescapedStringViaUrlDecode(SearchText), StringHelper.EmptyOrUnescapedStringViaUrlDecode(BasicInfoGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCertificationCategoryList()
        {
            string r = SQMCertification_CertificationInfo_Helper.GetCertificationCategoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CreateMember(SQMCertification_CertificationInfo DataItem)
        {
            DataItem.VendorCode = getMemberGUID();
            string r = SQMCertification_CertificationInfo_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string DeleteMember(SQMCertification_CertificationInfo DataItem)
        {
            DataItem.VendorCode = getMemberGUID();
            string r = SQMCertification_CertificationInfo_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string EditMember(SQMCertification_CertificationInfo DataItem)
        {
            DataItem.VendorCode = getMemberGUID();
            string r = SQMCertification_CertificationInfo_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem, "", "");
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string UploadIntroFile(string BasicInfoGUID, FileAttachmentInfo FA, String Type, String CertificationType)
        {
            string vendorCode = getMemberGUID();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SQMCertification_CertificationInfo_Helper.UploadIntroFile(
                    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                    FA,
                    vendorCode,
                    sLocalPath,
                    sLocalUploadPath,
                    Server,
                    Request.ApplicationPath,
                    CertificationType,
                    BasicInfoGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);

            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string getMemberGUID()
        {
            string r = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            return r;
            //string r = SQMCertification_CertificationInfo_Helper.GetMapVendorCode(
            //    DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
            //    PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
            //    );
            //DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            //return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticalFilesDetail()
        {
            string vendorCode = getMemberGUID();
            string r = SQMBasic_CriticalFile_Helper.GetCriticalFilesDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
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
    }
}
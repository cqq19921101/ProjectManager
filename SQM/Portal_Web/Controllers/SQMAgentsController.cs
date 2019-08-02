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

    public class SQMAgentsController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        [LogActionAccess]
        [SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]

        // GET: SQMAgents
        public ActionResult SQMAgents()
        {
            //Determin Upload Folder Name
            ViewBag.UploadFolderName = DateTime.Now.ToString("yyyyMMdd_HHmmss_") + Guid.NewGuid().ToString().Replace("-", "_");
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetVendorType()
        {
            string r = SystemMgmt_Agents_Helper.GetVendorType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );

            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBasicData()
        {
            string vendorCode = GetMapVendorCode();
            string r = SystemMgmt_Agents_Helper.GetBasicData(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCriticalFile()
        {
            string vendorCode = GetMapVendorCode();
            string r = SystemMgmt_Agents_Helper.GetCriticalFile(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                vendorCode
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetCommodityList()
        {
            string r = SystemMgmt_Agents_Helper.GetCommodityList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

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

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMapVendorCode()
        {
            string r = SystemMgmt_Agents_Helper.GetMapVendorCode(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext)
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

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
        public string UploadAgents(string BasicInfoGUID,FileAttachmentInfo FA,string PrincipalProducts)
        {
            string vendorCode = GetMapVendorCode();
            string sLocalPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, FA.SUBFOLDER);
            string sLocalUploadPath = FileHandleHelper.GetMappedLocalPath(Server, Request.ApplicationPath, sLocalPathBase, "");

            string r = SystemMgmt_Agents_Helper.UploadIntroFile(
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
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
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
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
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


    }
}
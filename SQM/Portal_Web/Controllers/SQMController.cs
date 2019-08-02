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
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Portal_Web.Controllers
{
    //[GetUserProfileViaWindowsAuthentication(Order = 10)]
    //[OverrideMemberCulture(Order = 20)]
    //[Authorize(Order = 30)]
    //[ControllerActionAccessControl(Order = 40)]
    //[UpdateSessionLastAccessTime(Order = 50)]
    //[OverrideMemberCulture]
    public class SQMController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        #region SQMBasic
        [AllowAnonymous]
        [SessionExpire_Service(Order = 1)]
        [SetPortalSessionStopWatch(Order = 60)]
        public ActionResult Approve(string TaskID)
        {
            ViewData["TaskID"] = TaskID;
            DataTable dt = SQM_Approve_Case_Helper.GetBasicInfoTypeByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID);
            DataRow dr = dt.Rows[0];
            //VTNAME,CNAME,CSNAME,NameInChinese,PlantCode,VendorCode
            ViewData["VTNAME"] = dr["VTNAME"].ToString();
            ViewData["CNAME"] = dr["CNAME"].ToString();
            ViewData["CSNAME"] = dr["CSNAME"].ToString();
            ViewData["NameInChinese"] = dr["NameInChinese"].ToString();
            ViewData["PlantCode"] = dr["PlantCode"].ToString();
            ViewData["VendorCode"] = dr["VendorCode"].ToString();
            ViewData["Status"] = dr["Status"].ToString();
            ViewData["TaskStatus"] = dr["TaskStatus"].ToString();
            ViewData["Type"] = dr["Type"].ToString();
            if (ViewData["Type"].ToString()=="1")
            {
                ViewData["Subj"] = "SQM基本調查表簽核";
            }else if (ViewData["Type"].ToString() == "2")
            {
                ViewData["Subj"] = "SQM供應商選定簽核";
            }
            else if (ViewData["Type"].ToString() == "3")
            {
                ViewData["Subj"] = "SQM供應商信赖性簽核";
            }
            else if (ViewData["Type"].ToString() == "4")
            {
                ViewData["Subj"] = "SQM供應商可靠度报告簽核";
            }
            else if (ViewData["Type"].ToString() == "5")
            {
                ViewData["Subj"] = "SQM供應商變更通知單簽核";
            }

            string appName = Request.ApplicationPath.ToString();
            string serverName = Request.Url.Authority.ToString();
            string protocol = Regex.Split(Request.Url.AbsoluteUri.ToString(), @"://")[0];
            string urlPre = protocol + "://" + serverName + appName;
            ViewData["FileURL"] = urlPre + "/SQM/DownloadSQMFile?DataKey=" + dr["FGUID"].ToString();
            ViewData["appPath2"] = urlPre;
            return View();
        }
        [AllowAnonymous]
        [SessionExpire_Service(Order = 1)]
        [SetPortalSessionStopWatch(Order = 60)]
        public ActionResult ReliabilityApprove(string TaskID)
        {
            ViewData["TaskID"] = TaskID;
            DataTable dt = SQM_Approve_Case_Helper.GetReliabilityByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID);
            DataRow dr = dt.Rows[0];
            ViewData["Status"] = dr["Status"].ToString();
            ViewData["TaskStatus"] = dr["TaskStatus"].ToString();
      
            ViewData["Subj"] = "SQM供應商信赖性簽核";
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            //ViewData["FileURL"] = urlPre + "/SQM/DownloadSQMFile?DataKey=" + dr["FGUID"].ToString();
            ViewData["appPath2"] = urlPre;
            return View();
        }
        [AllowAnonymous]
        [SessionExpire_Service(Order = 1)]
        [SetPortalSessionStopWatch(Order = 60)]
        public ActionResult ReliabilityFileApprove(string TaskID)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            ViewData["TaskID"] = TaskID;
            DataTable dt = SQM_Approve_Case_Helper.GetReliabilityFileByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID, urlPre);
            DataRow dr = dt.Rows[0];
            ViewData["Status"] = dr["Status"].ToString();
            ViewData["TaskStatus"] = dr["TaskStatus"].ToString();
          
            ViewData["Subj"] = "SQM供應商可靠度报告簽核";
          
            //ViewData["FileURL"] = urlPre + "/SQM/DownloadSQMFile?DataKey=" + dr["FGUID"].ToString();
            ViewData["appPath2"] = urlPre;
            return View();
        }

        [AllowAnonymous]
        [SessionExpire_Service(Order = 1)]
        [SetPortalSessionStopWatch(Order = 60)]
        public ActionResult ChangeApprove(string TaskID)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            ViewData["TaskID"] = TaskID;
            DataTable dt = SQM_Approve_Case_Helper.GetChangeByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID, urlPre);
            DataRow dr = dt.Rows[0];
            ViewData["Status"] = dr["Status"].ToString();
            ViewData["TaskStatus"] = dr["TaskStatus"].ToString();

            ViewData["Subj"] = "SQM供應商變更通知單簽核";
            
            //ViewData["FileURL"] = urlPre + "/SQM/DownloadSQMFile?DataKey=" + dr["FGUID"].ToString();
            ViewData["appPath2"] = urlPre;
            return View();
        }
        //[HttpPost]
        //[SessionExpire_Service(Order = 1)]
        [AllowAnonymous]
        public string UpdateTaskStatus(UpdateTaskData DataItem)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            DataItem.appName = urlPre;
            //測試階段用,測完將拿掉
            //SqlConnection sl = new SqlConnection("Data Source=ICM667;Initial Catalog=SPM;User ID=EA_APP;Pwd=jawa7trwe9;");
            //sl.Open();
            //DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB)
            String r = SQM_Approve_Case_Helper.UpdateTaskStatus(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), DataItem);
           // sl.Close();
            return r;
        }

        //[HttpGet]
        //[SessionExpire_Service(Order = 1)]
        [AllowAnonymous]
        public FileResult DownloadSQMFile(string DataKey)
        {
            FileInfoForOutput fi = SQMBasic_CriticalFile_Helper.DownloadSQMFileByStream(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                DataKey);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return File(fi.Buffer, fi.MimeMapping, FileHandleHelper.GetFixedFileName(Request.Browser, Server, fi.FileName));
        }
        #endregion

        [AllowAnonymous]
        public string LoadJSonWithFilter(string TaskID)
        {

            DataTable dt = SQM_Approve_Case_Helper.GetReliabilityByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID);
            string r = JsonConvert.SerializeObject(dt);
            return r;
        }
        [AllowAnonymous]
        public string LoadFileJSonWithFilter(string TaskID)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            urlPre += "/SQM/DownloadSQMFile?DataKey=";
            DataTable dt = SQM_Approve_Case_Helper.GetReliabilityFileByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID,urlPre);
         
                string r = JsonConvert.SerializeObject(dt);
            return r;
        }


        [AllowAnonymous]
        public string LoadChangeJSonWithFilter(string TaskID)
        {
            string urlPre = Lib_SQM_Domain.Modal.CommonHelper.getURL(Request);
            urlPre += "/SQM/DownloadSQMFile?DataKey=";
            DataTable dt = SQM_Approve_Case_Helper.GetChangeByTaskID(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.SPMDB), TaskID, urlPre);
            string r = JsonConvert.SerializeObject(dt);
            return r;
        }
    }
}

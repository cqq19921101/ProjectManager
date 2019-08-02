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
    public class SQMPVLController : Controller
    {
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
        public string GetUserGUID()
        {
            string r = PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID;
            return r;
        }


        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPVL( string VTNAME,string CNAME,string CSNAME)
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
             string r =SQM_PVL_Helper.EditPVL(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
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

    }
}
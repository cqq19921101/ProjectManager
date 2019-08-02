using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Portal_Web.SupportClasses;
using Lib_VMI_Domain.Model;
using Newtonsoft.Json;

namespace Portal_Web.Controllers
{
    [OverrideMemberCulture(Order = 20)]
    [Authorize(Order = 30)]
    [ControllerActionAccessControl(Order = 40)]
    [SetPortalSessionStopWatch(Order = 60)]
    public class VMIBulletinController : Controller
    {
        // GET: VMIBulletin
        public ActionResult Index()
        {
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

        #region BULLETIN_01_01: System Bulletin
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult RefAndDownload()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBulletinCategoryList()
        {
            string r = VMI_Bulletin_Helper.GetBulletinCategoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBulletinList(VMI_Bulletin_QueryItemForBulletinList queryItem, string page)
        {
            string r = VMI_Bulletin_Helper.GetBulletinList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem,
                page);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBulletinDetail(string ID)
        {
            string r = VMI_Bulletin_Helper.GetBulletinDetail(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetBulletinAttachment(string ID, string SortID)
        {
            string fi = VMI_Bulletin_Helper.GetBulletinAttachment(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               ID,
               SortID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order =1)]
        public string CheckPOAckInfo()
        {
            string r = VMI_Bulletin_Helper.CheckPOAckInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion
    }
}
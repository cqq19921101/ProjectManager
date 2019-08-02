using Lib_Portal_Domain.SharedLibs;
using Lib_Portal_Domain.SupportClasses;
using Lib_SQM_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal_Web.Controllers
{
    public class SQMBenefitController : Controller
    {
        // GET: SQMBenefit
        public ActionResult SQMBenefit()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string LoadJSonWithFilter(string SearchText)
        {
            string r = Benefit_Helper.GetDataToJQGridJson(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SearchText, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetSQMBenefit(string SID)
        {
            string r = Benefit_Helper.GetSQMBenefit(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), SID, StringHelper.EmptyOrUnescapedStringViaUrlDecode(PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID));
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Create(SQM_Benefit DataItem)
        {
            string r = Benefit_Helper.CreateDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Edit(SQM_Benefit DataItem)
        {
            string r = Benefit_Helper.EditDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string Delete(SQM_Benefit DataItem)
        {
            string r = Benefit_Helper.DeleteDataItem(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetSQMBenefitData(SQM_Benefit DataItem)
        {
            string r = Benefit_Helper.GetSQMBenefitData(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), DataItem,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetSQMAnnual(string MaterialType)
        {
            string r = Benefit_Helper.GetSQMAnnual(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB), MaterialType,
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext).MemberGUID);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExportMonth()
        {
            string fileName = "月度"+DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string r = Benefit_Helper.ExportMonth(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
               localPath, fileName);
           
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            r =  @"\\" + Request.Url.Authority.ToString()+@"\"+ Request.Url.Segments[1].Replace("/","") + @"\UploadFile\SQM\" + fileName;
            
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string ExportYear()
        {
            string fileName = "年度" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            string localPath = FileHandleHelper.GetMappedLocalAppRootPath(Server, Request.ApplicationPath);
            string r = Benefit_Helper.ExportYear(DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetLoginUserProfile(HttpContext).MemberGUID,
               localPath, fileName);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            r = @"\\" + Request.Url.Authority.ToString() + @"\" + Request.Url.Segments[1].Replace("/", "") + @"\UploadFile\SQM\" + fileName;
            return r;
        }
    }
}
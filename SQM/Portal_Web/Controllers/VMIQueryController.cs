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
    public class VMIQueryController : Controller
    {
        const string sLocalPathBase = "UploadFile/";

        // GET: VMIQuery
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

        #region VDS_02_XX Query VDS
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult QueryVDS()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQueryVDSList(VMI_Query_QueryItemForQueryVDSInfo QueryItem)
        {
            string r = VMI_Query_Helper.GetQueryVDSList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQueryVDSTMType(VMI_Query_QueryVDSTMTypeQueryItem QueryItem)
        {
            string r = VMI_Query_Helper.GetQueryVDSTMType(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQueryVDSHeader(VMI_Process_QueryItemForVDSHeader QueryItem)
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
        public string GetQueryVDSDetail(VMI_Process_QueryItemForVDSDetail QueryItem, string page)
        {
            string r = VMI_Process_Helper.GetVDSDetailJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem,
                page,
                false,
                VMI_Process_VDSFunc.QueryVDS);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryVDSExportVDS(VMI_Process_QueryItemForToDoVDS queryItem)
        {
            string fi = VMI_Query_Helper.QueryVDSExportVDSInfo(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                queryItem,
                VMI_Process_VDSFunc.QueryVDS);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQueryVDSVerCompareDetail(VMI_Process_QueryItemForVDSDetail QueryItem, string page)
        {
            string r = VMI_Query_Helper.GetVDSVerCompareDetailJson(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem,
                page,
                false,
                VMI_Process_VDSFunc.QueryVDS);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Inventory_01_XX: Query Inventory
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult QueryDailyInventory()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetQueryDailyInventoryList(VMI_Query_QueryItemForQueryDailyInventoryList QueryItem, string page)
        {
            string r = VMI_Query_Helper.GetQueryDailyInventoryList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem,
                page);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryDailyInventoryjqGrid(VMI_Query_QueryItemForQueryDailyInventoryList QueryItem, string page)
        {
            string r = VMI_Query_Helper.QueryDailyInventoryjqGrid(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem,
                page);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetMaterialDoc(VMI_Query_QueryItemForGetMaterialDoc QueryItem, string page)
        {
            string r = VMI_Query_Helper.GetMaterialDoc(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryDailyInventoryExportExcel(VMI_Query_QueryItemForQueryDailyInventoryList QueryItem)
        {
            string fi = VMI_Query_Helper.QueryDailyInventoryExportExcel(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region QueryPayment_01_01: Query Payment Status
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult APSearch()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryAPSearchVendorList(VMI_Query_QueryItemForAPSearch QueryItem)
        {
            List<string> r = VMI_Query_Helper.QueryAPSearchVendorList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return JsonConvert.SerializeObject(r);
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryAPSearch(VMI_Query_QueryItemForAPSearch QueryItem)
        {
            string r = VMI_Query_Helper.QueryAPSearchJqGrid(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string APSearchExportAllExcel(VMI_Query_QueryItemForAPSearch QueryItem)
        {
            string fi = VMI_Query_Helper.APSearchExport(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                QueryItem,
                VMI_Query_APSearchExportFunc.ExportAllExcel);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string APSearchExportAllPDF(VMI_Query_QueryItemForAPSearch QueryItem)
        {
            string fi = VMI_Query_Helper.APSearchExport(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                QueryItem,
                VMI_Query_APSearchExportFunc.ExportAllPDF);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string APSearchExportExcel(VMI_Query_QueryItemForAPSearch QueryItem)
        {
            string fi = VMI_Query_Helper.APSearchExport(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                QueryItem,
                VMI_Query_APSearchExportFunc.ExportExcel);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region GRINFO_01_01: GRInfo to Excel
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult GRReport()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GRReportExportExcel(VMI_Query_QueryItemforGRReport QueryItem)
        {
            string fi = VMI_Query_Helper.GRReportExportExcel(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region ASN_02_01: Query ASN
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult QueryASN()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNInfoJsonWithFilter(VMI_Query_QueryItemForQueryASN QueryItem)
        {
            string r = VMI_Query_Helper.Query_ASNList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNInfoToExcel(string ASNNUM)
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
        public string EditImportDate(string ASN_NUM, string ARRIVAL_DATE, string PLAN_IMPORT_DATE)
        {
            string r = VMI_Query_Helper.EditImportDate(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                ASN_NUM,
                ARRIVAL_DATE,
                PLAN_IMPORT_DATE
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string PrintImportedNoticeForm(string ASNNUM)
        {
            string fi = VMI_Query_Helper.PrintImportedNoticeForm(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                Server,
                Request.ApplicationPath,
                ASNNUM
                );
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region NormalPOGR_01_01: Export Normal PO GR
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult POAPCheckStatement()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPOAPCheckExcelFileList(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string r = VMI_Query_Helper.GetPOAPCheckExcelFileList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string POAPCheckExportExcel(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string fi = VMI_Query_Helper.POAPCheckExportExcel(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPOAPCheckPDFFileList(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string r = VMI_Query_Helper.GetPOAPCheckPDFFileList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string POAPCheckExportPDF(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string fi = VMI_Query_Helper.POAPCheckExportPDF(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion

        #region ConsignmentGI_01_01 Export Consignment GI
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult APCheckStatement()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAPCheckExcelFileList(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string r = VMI_Query_Helper.GetAPCheckExcelFileList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string APCheckExportExcel(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string fi = VMI_Query_Helper.APCheckExportExcel(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetAPCheckPDFFileList(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string r = VMI_Query_Helper.GetAPCheckPDFFileList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string APCheckExportPDF(VMI_Query_QueryItemForAPCheck QueryItem)
        {
            string fi = VMI_Query_Helper.APCheckExportPDF(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }        
        #endregion

        #region ASN_04_01 : ASN Report
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult ASNReport()
        {
            return View();
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryASNReportInfoJsonWithFilter(VMI_Query_QueryItemForQueryASN QueryItem)
        {
            string r = VMI_Query_Helper.Query_ASNReportList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string CheckASNReportType(string ASNNUMs)
        {
            string r = VMI_Query_Helper.Check_ASNReportIsDiff(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                ASNNUMs);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region QueryPOAck
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult QueryPOAck()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string QueryPOAckList(ToDoPOAckQueryItem queryItem)
        {
            string r = VMI_Query_Helper.QueryPOAckListJqGrid(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                queryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        #endregion

        #region Period Inventory Report - Consignment
        [GetUserProfileViaWindowsAuthentication(Order = 0)]
        //[SessionExpire_Page(Order = 10)]
        [SetFunctionPathValueForPortal(Order = 20)]
        public ActionResult PerInvRptConsign()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string GetPeriodInventoryReportPDFFileList(VMI_Query_QueryItemForPerInvRptConsign QueryItem)
        {
            string r = VMI_Query_Helper.GetPeriodInventoryReportPDFFileList(
                DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
                PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
                QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return r;
        }
        [HttpPost]
        [SessionExpire_Service(Order = 1)]
        public string PeriodInventoryReportExportPDF(VMI_Query_QueryItemForPerInvRptConsign QueryItem)
        {
            string fi = VMI_Query_Helper.PeriodInventoryReportExportPDF(
               DBConnHelper.GetPortalDBConnectionOpen(Session, PortalDBType.PortalDB),
               PortalUserProfileHelper.GetRunAsUserProfile(HttpContext),
               Server,
               Request.ApplicationPath,
               QueryItem);
            DBConnHelper.ClosePortalDBConnection(Session, PortalDBType.PortalDB);
            return fi;
        }
        #endregion
    }
}
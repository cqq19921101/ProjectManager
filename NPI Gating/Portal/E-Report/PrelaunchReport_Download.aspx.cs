using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using LiteOn.EA.DAL;
using LiteOn.EA.BLL;
using System.Text;
using LiteOn.EA.NPIReport.Utility;

public partial class Web_E_Report_PrelaunchReport_Download : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string caseID = Context.Request.QueryString["caseID"].ToString();
            string Bu = Context.Request.QueryString["Bu"].ToString();
            string FormNo = Context.Request.QueryString["FormNo"].ToString();
            //string caseID ="306571";
            //string Bu ="POWER";
            //string FormNo = "PRE201801-00060";
            ExportXLS(caseID,Bu,FormNo);

        }
    }

    private void ExportXLS(string caseID, string Bu,string FormNo)
    {
        string templatePath = string.Empty;

        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);

        templatePath = Page.Server.MapPath("~/Web/E-Report/TempPrelaunch .xlsx");
        //Instantiate a new Workbook object.
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
      
        Aspose.Cells.Worksheet sheet1 = book.Worksheets[0];
        Aspose.Cells.Worksheet sheet2 = book.Worksheets[1];
 
         // REPLACE PUBLIC FIELDS
         BindHomePage(ref sheet1, caseID,Bu);
         BindCheckItem(ref sheet2, FormNo, Bu);

        this.Response.Clear();
        book.Save("Prelaunch_Report.xls",
         Aspose.Cells.FileFormatType.Excel97To2003,
         Aspose.Cells.SaveType.OpenInExcel,
         this.Response,
         System.Text.Encoding.UTF8);
    }


   private void BindCheckItem(ref Aspose.Cells.Worksheet sheet, string FormNo, string Bu)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
        Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetPrelaunchInconformity(FormNo);
        if (dtMaster.Rows.Count > 0)
        {
            int templateIndex=1;//模板row起始位置
            int insertIndexEnCounter = templateIndex + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndex, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            string url = "http://icm651.liteon.com/WF_PrelaunchReport/";
            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {

                DataRow dr = dtMaster.Rows[i];
                string FileName = dr["FileName"].ToString();
                cells[i + templateIndex, 0].PutValue(dr["ID"].ToString());
                cells[i + templateIndex, 1].PutValue(dr["Dept"].ToString());
                cells[i + templateIndex, 2].PutValue(dr["CheckItem"].ToString());
                cells[i + templateIndex, 3].PutValue(dr["Description"].ToString());
                cells[i + templateIndex, 4].PutValue(dr["Status"].ToString());
                cells[i + templateIndex, 5].PutValue(dr["Remark"].ToString());
                cells[i + templateIndex, 6].PutValue(dr["Suggestion"].ToString());
                cells[i + templateIndex, 7].PutValue(dr["CompleteDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["CompleteDate"].ToString()).ToString("yyyy/MM/dd") : dr["CompleteDate"].ToString());
                cells[i + templateIndex, 8].PutValue(dr["UpateUser"].ToString());
                cells[i + templateIndex, 9].PutValue(dr["UpdateTime"].ToString().Length > 0 ? Convert.ToDateTime(dr["UpdateTime"].ToString()).ToString("yyyy/MM/dd") : dr["UpdateTime"].ToString());

                if (!string.IsNullOrEmpty(FileName))
                {
                    string destFileName = url + dr["AttacheFile"].ToString();
                    style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
                    cells[i + templateIndex, 10].PutValue(FileName);
                    cells[i + templateIndex, 10].SetStyle(style);
                    sheet.Hyperlinks.Add(i + templateIndex, 10, 1, 1, destFileName);
                    cells.Merge(i + templateIndex, 10, 1, 3);
                }
                
            }


        }

        #endregion

    }


    private void BindHomePage(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
       

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();
       
        #region 主檔資訊

        DataTable dt = oStandard.GetPrelaunchMaster(caseID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            sheet.Replace("{FormNo}", dr["PilotRunNO"].ToString());
            sheet.Replace("{Model}", dr["Model"].ToString());
            sheet.Replace("{Customer}", dr["Customer"].ToString());
            sheet.Replace("{PCBRev}", dr["PCBInRev"].ToString());
            sheet.Replace("{Rev}", dr["PLRev"].ToString());
            sheet.Replace("{Date}", Convert.ToDateTime(dr["Date"].ToString()).ToString("yyyy/MM/dd"));
            sheet.Replace("{TPME}", dr["TP_ME"].ToString());
            sheet.Replace("{TPEE}", dr["TP_EE"].ToString());
            sheet.Replace("{TPPM}", dr["TP_PM"].ToString());
            sheet.Replace("{PM}", dr["PM"].ToString());
            sheet.Replace("{PCBORev}", dr["PCBOutRev"].ToString());
            sheet.Replace("{Attection}", dr["Notes"].ToString());
            sheet.Replace("{WorkOrder}", dr["WorkOrder"].ToString());
        }

        DataTable dtResult = GetNPIManager(caseID);
        DataRow drResult = dtResult.Rows[0];
        sheet.Replace("{FinalResult}", drResult["APPROVE_RESULT"].ToString() == "Y" ? "Pass" : "Fail");
        #endregion

        #region 簽核資訊
        DataTable dtApprover = oStandard.GetPrelaunchApproveResult(caseID);
        if (dtApprover.Rows.Count > 0)
        {
            int templateIndex = 17;//模板row起始位置
            int insertIndexEnCounter = templateIndex + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtApprover.Rows.Count - 1);
            cells.CopyRows(cells, templateIndex, insertIndexEnCounter, dtApprover.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtApprover.Rows.Count; i++)
            {

                DataRow dr = dtApprover.Rows[i];
                cells[i + templateIndex, 0].PutValue(dr["STEP_NAME"].ToString());
                cells[i + templateIndex, 1].PutValue(dr["DEPT"].ToString());
                cells[i + templateIndex, 2].PutValue(dr["HANDLER"].ToString());
                cells[i + templateIndex, 3].PutValue(dr["APPROVE_TIME"].ToString().Length > 0 ? Convert.ToDateTime(dr["APPROVE_TIME"].ToString()).ToString("yyyy/MM/dd") : dr["APPROVE_TIME"].ToString());
                cells[i + templateIndex, 4].PutValue(dr["APPROVE_RESULT"].ToString().Length > 0 ? dr["APPROVE_RESULT"].ToString() : "Y");
                cells[i + templateIndex, 5].PutValue(dr["APPROVE_REMARK"].ToString());


            }
        }


        #endregion

    }

    private DataTable GetNPIManager(string caseid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("	SELECT  *FROM TB_Prelaunch_Step_Handler  with(nolock) ");
        sb.Append("	 WHERE  CASEID=@CaseId and STEP_NAME = 'NPI Top Manager'");
        opc.Add(DataPara.CreateDataParameter("@CaseId",SqlDbType.VarChar,caseid));
        DataTable dt = sdb.GetDataTable(sb.ToString(), opc);
        return dt;
    }


    /// <summary>
    /// 設定頁面打印格式
    /// </summary>
    /// <param name="sheet">worksheet</param>
    /// <param name="type">列印模式：直印，橫印</param>
    private void SetStyle(ref Aspose.Cells.Worksheet sheet, Aspose.Cells.PageOrientationType type)
    {

        sheet.PageSetup.IsPercentScale = false;
        sheet.PageSetup.FitToPagesWide = 1; //自動縮放為一頁寬
        sheet.PageSetup.LeftMargin = 0.5;
        sheet.PageSetup.RightMargin = 0.5;
        sheet.PageSetup.TopMargin = 0.5;
        sheet.PageSetup.BottomMargin = 0.5;
        sheet.PageSetup.Orientation = type;

    }


    /// <summary>
    /// 設定表頁的列寬自適應
    /// </summary>
    /// <param name="sheet"></param>
    private void SetColumnAuto(ref Aspose.Cells.Worksheet sheet)
    {
        Aspose.Cells.Cells cells = sheet.Cells;

        //获取页面最大列数
        int columnCount = cells.MaxColumn + 1;

        //获取页面最大行数
        int rowCount = cells.MaxRow;
        for (int col = 0; col < columnCount; col++)
        {
            sheet.AutoFitColumn(col, 0, rowCount);
        }
        for (int col = 0; col < columnCount; col++)
        {
            int pixel = cells.GetColumnWidthPixel(col) + 10;
            if (pixel > 255)
            {
                cells.SetColumnWidthPixel(col, 255);
            }
            else
            {
                cells.SetColumnWidthPixel(col, pixel);
            }
        }

    }

 
}


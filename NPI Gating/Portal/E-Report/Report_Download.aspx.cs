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
public partial class Report_Download : System.Web.UI.Page
{
    SqlDB sdb = new SqlDB(DataPara.GetDbConnectionString("NPI_REPORT"));
    ArrayList opc = new ArrayList();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string docno = Context.Request.QueryString["DocNo"].ToString();
            string caseID = Context.Request.QueryString["caseID"].ToString();
            string Bu = Context.Request.QueryString["Bu"].ToString();
            string Building = Context.Request.QueryString["Building"].ToString();

            //string docno ="EVT201712-01000";
            //string caseID ="305465";
            //string Bu ="POWER";
            //string Building ="A3";
            ExportXLS(docno, caseID,Bu,Building);
           
        }
    }

    private void ExportXLS(string docno, string caseID, string Bu,string Building)
    {
        string templatePath = string.Empty;
     
        Aspose.Cells.License lic = new Aspose.Cells.License();
        string AsposeLicPath = System.Configuration.ConfigurationSettings.AppSettings["AsposeLicPath"].ToString();
        lic.SetLicense(AsposeLicPath);

        templatePath = Page.Server.MapPath("~/Web/E-Report/ReportTemp.xlsx");
        //Instantiate a new Workbook object.
        Aspose.Cells.Workbook book = new Aspose.Cells.Workbook(templatePath);
        Aspose.Cells.Worksheet sheet1 = book.Worksheets[0];//DFX
        Aspose.Cells.Worksheet sheet2 = book.Worksheets[1];// PMFEA
         Aspose.Cells.Worksheet sheet3 = book.Worksheets[2];// IssuesList
         Aspose.Cells.Worksheet sheet4 = book.Worksheets[3];// CTQ
        // REPLACE PUBLIC FIELDS
        BindExcel(ref sheet1, caseID,Bu,Building,docno);
        BindPFMA(ref sheet2, caseID, Bu, Building, docno);
        BindIssuesList(ref sheet3, caseID, Bu, Building, docno);
        BindCTQ(ref sheet4, caseID, Bu, Building, docno);
        SetColumnAuto(ref sheet1);
         this.Response.Clear();
             book.Save("NPI_Report.xls",
              Aspose.Cells.FileFormatType.Excel97To2003,
              Aspose.Cells.SaveType.OpenInExcel,
              this.Response,
              System.Text.Encoding.UTF8);
    }


    /// <summary>
    /// 填充頁面數據
    /// </summary>
    /// <param name="sheet">worksheet</param>
    /// <param name="docno">試產主單號</param>
    /// <param name="subdocno">試產從單號</param>
    private void BindExcel(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building,string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);
       
        NPIMgmt oMgmt = new NPIMgmt("CZ",Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetDFXInconformity(DocNo,"","");

        //string xmlReason = string.Empty;
        //string xmlPMC = string.Empty;
        //string xmlResult = string.Empty;
        //string xmlReasonDetail = string.Empty;
        if (dtMaster.Rows.Count > 0)
        {
           int templateIndexDFX=5;//模板row起始位置
           int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

           cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
           cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

           for (int i = 0; i < dtMaster.Rows.Count; i++)
           {
              
               DataRow dr = dtMaster.Rows[i];
               cells[i + templateIndexDFX, 1].PutValue(dr["ItemType"].ToString());
               cells[i + templateIndexDFX, 2].PutValue(dr["Item"].ToString());
               cells[i + templateIndexDFX, 3].PutValue(dr["Location"].ToString());
               cells[i + templateIndexDFX, 4].PutValue(dr["Requirements"].ToString());
               cells[i + templateIndexDFX, 5].PutValue("");
               cells[i + templateIndexDFX, 6].PutValue(dr["Compliance"].ToString());
               cells[i + templateIndexDFX, 7].PutValue(dr["PriorityLevel"].ToString());
               cells[i + templateIndexDFX, 8].PutValue(dr["MaxPoints"].ToString());
               cells[i + templateIndexDFX, 9].PutValue(dr["DFXPoints"].ToString());
               cells[i + templateIndexDFX, 10].PutValue(dr["Comments"].ToString());
               cells[i + templateIndexDFX, 11].PutValue(dr["Actions"].ToString());
               cells[i + templateIndexDFX, 12].PutValue(dr["CompletionDate"].ToString().Length>0 ? Convert.ToDateTime(dr["CompletionDate"].ToString()).ToString("yyyy/MM/dd"):dr["CompletionDate"].ToString());
               cells[i + templateIndexDFX, 13].PutValue(dr["Tracking"].ToString());
               cells[i + templateIndexDFX, 14].PutValue(dr["Remark"].ToString());
               cells[i + templateIndexDFX, 15].PutValue(dr["WriteDept"].ToString());

             
           }


        }

        #endregion

     
    }



    private void BindPFMA(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetFMEAInconformity(DocNo, "", "", "");

        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {

                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 1].PutValue(dr["Item"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["Stantion"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["Source"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["Source"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["PotentialFailureMode"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["Loess"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["Loess"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["Sev"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["Occ"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["DET"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["RPN"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["PotentialFailure"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["TargetCompletionDate"].ToString().Length > 0 ? Convert.ToDateTime(dr["TargetCompletionDate"].ToString()).ToString("yyyy/MM/dd") : dr["TargetCompletionDate"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["ActionsTaken"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["ResultsSev"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["ResultsOcc"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["ResultsDet"].ToString());
                cells[i + templateIndexDFX, 17].PutValue(dr["ResultsRPN"].ToString());
                cells[i + templateIndexDFX, 18].PutValue(dr["WriteDept"].ToString());


            }


        }

        #endregion

    }


    private void BindIssuesList(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetIssuesInconformity(DocNo, "", "");
        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 6;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {

                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 1].PutValue(dr["Items"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["STATION"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["ISSUE_DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["FILE_PATH"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["ISSUE_LOSSES"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["TEMP_MEASURE"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["IMPROVE_MEASURE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["CURRENT_STATUS"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["TRACKING"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["REMARK"].ToString());
            }


        }

        #endregion

    }


    private void BindCTQ(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetCLCAInconformity(DocNo, "", "");
        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX =7;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {

                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 1].PutValue(dr["PROCESS"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["CTQ"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["CONTROL_TYPE"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["ACT"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["RESULT"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["ROOT_CAUSE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["D"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["M"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["P"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["E"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["W"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["O"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["TEMPORARY_ACTION"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["CORRECTIVE_PREVENTIVE_ACTION"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["COMPLETE_DATE"].ToString().Length > 0 ? Convert.ToDateTime(dr["COMPLETE_DATE"].ToString()).ToString("yyyy/MM/dd") : dr["COMPLETE_DATE"].ToString());
                cells[i + templateIndexDFX, 17].PutValue(dr["IMPROVEMENT_STATUS"].ToString());

            }


        }

        #endregion

    }



    private void BindHomePage(ref Aspose.Cells.Worksheet sheet, string caseID, string Bu, string Building, string DocNo)
    {
        //page 格式設定
        SetStyle(ref sheet, Aspose.Cells.PageOrientationType.Landscape);
        Aspose.Cells.Cells cells = sheet.Cells;
        //string logoPath = Page.Server.MapPath("") + "\\log.png";
        //sheet.Pictures.Add(0, 0, 4, 10, logoPath);

        NPIMgmt oMgmt = new NPIMgmt("CZ", Bu);
        NPI_Standard oStandard = oMgmt.InitialLeaveMgmt();

        #region//獲取主表資訊
        DataTable dtMaster = oStandard.GetCLCAInconformity(DocNo, "", "");
        if (dtMaster.Rows.Count > 0)
        {
            int templateIndexDFX = 7;//模板row起始位置
            int insertIndexEnCounter = templateIndexDFX + 1;//new row起始位置

            cells.InsertRows(insertIndexEnCounter, dtMaster.Rows.Count - 1);
            cells.CopyRows(cells, templateIndexDFX, insertIndexEnCounter, dtMaster.Rows.Count - 1); //複製模板row格式至新行

            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {

                DataRow dr = dtMaster.Rows[i];
                cells[i + templateIndexDFX, 1].PutValue(dr["PROCESS"].ToString());
                cells[i + templateIndexDFX, 2].PutValue(dr["CTQ"].ToString());
                cells[i + templateIndexDFX, 3].PutValue(dr["CONTROL_TYPE"].ToString());
                cells[i + templateIndexDFX, 4].PutValue(dr["ACT"].ToString());
                cells[i + templateIndexDFX, 5].PutValue(dr["RESULT"].ToString());
                cells[i + templateIndexDFX, 6].PutValue(dr["DESCRIPTION"].ToString());
                cells[i + templateIndexDFX, 7].PutValue(dr["ROOT_CAUSE"].ToString());
                cells[i + templateIndexDFX, 8].PutValue(dr["D"].ToString());
                cells[i + templateIndexDFX, 9].PutValue(dr["M"].ToString());
                cells[i + templateIndexDFX, 10].PutValue(dr["P"].ToString());
                cells[i + templateIndexDFX, 11].PutValue(dr["E"].ToString());
                cells[i + templateIndexDFX, 12].PutValue(dr["W"].ToString());
                cells[i + templateIndexDFX, 13].PutValue(dr["O"].ToString());
                cells[i + templateIndexDFX, 14].PutValue(dr["TEMPORARY_ACTION"].ToString());
                cells[i + templateIndexDFX, 15].PutValue(dr["CORRECTIVE_PREVENTIVE_ACTION"].ToString());
                cells[i + templateIndexDFX, 16].PutValue(dr["COMPLETE_DATE"].ToString().Length > 0 ? Convert.ToDateTime(dr["COMPLETE_DATE"].ToString()).ToString("yyyy/MM/dd") : dr["COMPLETE_DATE"].ToString());
                cells[i + templateIndexDFX, 17].PutValue(dr["IMPROVEMENT_STATUS"].ToString());

            }


        }

        #endregion

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
            int pixel = cells.GetColumnWidthPixel(col) +10;
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

